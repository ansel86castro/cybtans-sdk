namespace Cybtans.Expressions.Ast
{
    public abstract class DeclarationNode:ASTNode
    {
        public DeclarationNode(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }
    }
}