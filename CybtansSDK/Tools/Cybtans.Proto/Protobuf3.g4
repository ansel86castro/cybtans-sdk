/**
 * A Protocol Buffers 3 grammar for ANTLR v4.
 *
 * Derived and adapted from:
 * https://developers.google.com/protocol-buffers/docs/reference/proto3-spec
 *
 */
grammar Protobuf3;

options {
	language='CSharp';    
}


@lexer::namespace {
	Cybtans.Proto
}

@parser::namespace {
	Cybtans.Proto
}

@header{
using Cybtans.Proto.AST;
}

@parser::members {

}

//
// Proto file
//

proto returns[ ProtoFile file =new ProtoFile()]
    :  syntax (  
       importStatement  {$file.Imports.Add($importStatement.node);} 
    |  packageStatement {$file.Package=$packageStatement.node;} 
    |  option           {$file.Options.Add($option.node);} 
    |  topLevelDef      {$file.Declarations.Add($topLevelDef.node);} 
    |   emptyStatement
    )*
        EOF
    ;

//
// Syntax
//

syntax
    :   'syntax' '=' ('"proto3"' | '\'proto3\'' ) ';'
    ;

//
// Import Statement
//

importStatement returns [ImportDeclaration node]
    :   'import' (w='weak' | p='public')? v=StrLit ';'
        {$node = new ImportDeclaration($start, $w != null?ImportType.Weak:ImportType.Public, $v.text);}
    ;

//
// Package
//

packageStatement returns[PackageDeclaration node]
    :   'package' id=fullIdent ';' { $node = new PackageDeclaration($id.start, $id.node);}
    ;

//
// Option
//

option returns[OptionsExpression node]
    locals[ExpressionNode e]
    :   'option' id=optionName '=' (constant{ $e=$constant.node;} | optionBody{$e = $optionBody.node;})  ';' {$node = new OptionsExpression($id.node, $e, $start);}
    ;

optionName returns[IdentifierExpression node]         
    :   (id=Ident { $node = new OptionIdentifierExpression($id.text, null, $start); }  
        |'(' fullIdent { 
              var n = new OptionIdentifierExpression($fullIdent.node.Id, $fullIdent.node.Left, $start );
              n.IsExtension = true;
              $node = n;
              } 
         ')' ) ('.' (id=Ident { $node = new OptionIdentifierExpression($id.text, $node, $id); } ))*
    ;

optionBody returns [InitializerExp node]
    : '{'
        ( e=optionBodyVariable{ 
            $node = $node??new InitializerExp($start); 
            $node.Expressions.Add($e.node); })*
      '}'
    ;

optionBodyVariable returns [MemberInitializerExp node]
    : id=Ident ':' e=constant {$node = new MemberInitializerExp($start,$id.text, $e.node);}
    ;

//
// Top Level definitions
//

topLevelDef returns[DeclarationNode node]
   :   message {$node = $message.node;}
   |   enumDefinition {$node = $enumDefinition.node; }
   |   service {$node = $service.node;}
   ;

// Message definition

message returns[MessageDeclaration node]
    :'message' messageName {$node = new MessageDeclaration($start, $messageName.text);}
    '{' (     e=enumDefinition {$node.Enums.Add($e.node);}
            |  m=message {$node.InnerMessages.Add($m.node);}
            |   option   {$node.Options.Add($option.node);}           
            |   mapField {$node.Fields.Add($mapField.node);}
            |   reserved
            |   field {$node.Fields.Add($field.node);}
            |   emptyStatement
            )*
       '}'
    ;

// Enum definition

enumDefinition returns[EnumDeclaration node]
    :   'enum' Ident {$node = new EnumDeclaration($Ident.text, $start);}
        '{' (   option {$node.Options.Add($option.node);}
            |   enumField {$node.Members.Add($enumField.node);}
            |   emptyStatement
            )*
        '}'
    ;

enumField returns[EnumMemberDeclaration node]
    :   Ident {$node =new EnumMemberDeclaration($Ident.text, $start);} '=' (neg='-')? IntLit {$node.Value = ($neg!=null?-1:1) * $IntLit.int;} ('[' o=fieldOptions {$node.Options.AddRange($o.value);} ']')? ';'
    ;


// Service definition

service returns[ServiceDeclaration node]
    :   'service' Ident {$node = new ServiceDeclaration($Ident.text, $start);} '{' 
        (   option {$node.Options.Add($option.node);}
            |   rpc {$node.Rpcs.Add($rpc.node);}         
            |   emptyStatement
        )*
        '}'
    ;

rpc returns[RpcDeclaration node]
    :   'rpc' Ident '(' 'stream'? req=fullIdent ')' 'returns' '(' 'stream'? resp=fullIdent ')' {
            $node = new RpcDeclaration($Ident.text, $req.node, $resp.node, $start);            
        } 
        ('{' (option{ $node.Options.Add($option.node);})* '}' ';'? | ';') 
    ;

//
// Reserved
//

reserved
    :   'reserved' (ranges | fieldNames) ';'
    ;

ranges
    :   range (',' range)*
    ;

    range
    :   IntLit
    |   IntLit 'to' IntLit
    ;

fieldNames
    :   StrLit (',' StrLit)*
    ;

fullIdent returns [IdentifierExpression node]
    :   id=Ident { $node=new IdentifierExpression($id.text,null,$id);} ('.' id=Ident {$node=new IdentifierExpression($id.text,$node,$id);})*  
    ;

// Empty Statement

emptyStatement
    :   ';'
    ;

// Constant

constant returns[ConstantExp node]
    :  
        (neg='-' | '+')? IntLit {$node = new ConstantExp($start, ($neg!=null?-1:1) * int.Parse($IntLit.text));}
    |   (neg='-' | '+')? FloatLit {$node = new ConstantExp($start, ($neg!=null?-1:1) * double.Parse($FloatLit.text));}
    |   StrLit {$node = new ConstantExp($start, $StrLit.text.Substring(1, $StrLit.text.Length - 2));}
    |   'true'  {$node = new ConstantExp($start, true);}
    |   'false' {$node = new ConstantExp($start, false);}        
    ;    
//
// Fields
//

type returns [IdentifierExpression node]
    : fullIdent {$node = $fullIdent.node;}
    ;

// Normal field

field returns[FieldDeclaration node]
@init
{ 
    TypeIdentifier typeRef = new TypeIdentifier();
    List<OptionsExpression> options = null;
}
    :   ('repeated'{typeRef.IsArray=true;})? type { typeRef.Name=$type.node; } (id=Ident | id = 'message' | id= 'service' | id = 'rpc') '=' number=IntLit ('['fieldOptions{ options=$fieldOptions.value; } ']')? ';'
        {
            $node = new FieldDeclaration($start, typeRef, $id.text, $number.int, options);
        }
    ;

fieldOptions returns[List<OptionsExpression>value = new List<OptionsExpression>()]
    :   o=fieldOption {$value.Add($o.node);} (','  o=fieldOption {$value.Add($o.node);} )*
    ;

fieldOption returns[OptionsExpression node]
    :   optionName '=' constant {$node=new OptionsExpression($optionName.node, $constant.node, $start);}
    ;

// Oneof and oneof field

oneof
    :   'oneof' oneofName '{' (oneofField | emptyStatement)* '}'
    ;

oneofField
    :   type fieldName '=' IntLit ('[' fieldOptions ']')? ';'
    ;

// Map field

mapField returns[FieldDeclaration node]
@init
{ 
    TypeIdentifier typeRef = new TypeIdentifier();
    typeRef.IsMap = true;
    typeRef.Name = new IdentifierExpression("map");

    List<OptionsExpression> options = null;
}
    :   'map' '<' k=type ',' v=type '>' Ident '=' IntLit ('[' fieldOptions{  options=$fieldOptions.value; } ']')? ';'
    {
        typeRef.GenericArgs = new TypeIdentifier[]{ new TypeIdentifier($k.node), new TypeIdentifier($v.node) };
        
        $node = new FieldDeclaration($start);        
        $node.Type = typeRef;
        $node.Name = $Ident.text;
        $node.Number= $IntLit.int;

        if(options!=null)
            $node.Options.AddRange(options);     
    }
    ;


//
// Lexical elements
//


// Letters and digits

fragment
Letter
    :   [A-Za-z_]
    ;

fragment
DecimalDigit
    :   [0-9]
    ;

fragment
OctalDigit
    :   [0-7]
    ;

fragment
HexDigit
    :   [0-9A-Fa-f]
    ;

// Identifiers

Ident
    :   Letter (Letter | DecimalDigit)*
    ;

messageName
    :   Ident
    ;

fieldName
    :   Ident
    ;

oneofName
    :   Ident
    ;

messageType
    :   '.'? (Ident '.')* messageName
    ;

// Integer literals

IntLit
    :   DecimalLit
    |   OctalLit
    |   HexLit
    ;

fragment
DecimalLit
    :   [1-9] DecimalDigit*
    ;

fragment
OctalLit
    :   '0' OctalDigit*
    ;

fragment
HexLit
    :   '0' ('x' | 'X') HexDigit+
    ;

// Floating-point literals

FloatLit
    :   (   Decimals '.' Decimals? Exponent?
        |   Decimals Exponent
        |   '.' Decimals Exponent?
        )
    |   'inf'
    |   'nan'
    ;

fragment
Decimals
    :   DecimalDigit+
    ;

fragment
Exponent
    :   ('e' | 'E') ('+' | '-')? Decimals
    ;

// Boolean

BoolLit
    :   'true'
    |   'false'
    ;

// String literals

StrLit
    :   '\'' CharValue* '\''
    |   '"' (HexEscape|OctEscape|CharEscape|~["\u0000\n\\])* '"'
    ;

fragment
CharValue
    :   HexEscape|OctEscape|CharEscape|~[\u0000\n\\]
    ;

fragment
HexEscape
    :   '\\' ('x' | 'X') HexDigit HexDigit
    ;

fragment
OctEscape
    :   '\\' OctalDigit OctalDigit OctalDigit
    ;

fragment
CharEscape
    :   '\\' [abfnrtv\\'"]
    ;

Quote
    :   '\''
    |   '"'
    ;

// Whitespace and comments

WS  :   [ \t\r\n\u000C]+ -> skip
    ;

COMMENT
    :   '/*' .*? '*/' -> skip
    ;

LINE_COMMENT
    :   '//' ~[\r\n]* -> skip
    ;
