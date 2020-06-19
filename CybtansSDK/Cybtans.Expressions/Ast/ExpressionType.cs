using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cybtans.Expressions.Ast
{
    public class ExpressionType {
	
	public static readonly int STD_Bool = 1;
	public static readonly  int STD_Integer= 2;
	public static readonly  int STD_Double= 3;
	public static readonly  int STD_String= 4;
	public static readonly  int STD_Null= 5;
    public static readonly int STD_TABLE = 6;
    public static readonly int STD_CUSTOM = 7;

    public static readonly  ExpressionType Bool = new ExpressionType("bool", STD_Bool);
	public static readonly  ExpressionType Integer= new ExpressionType("integer", STD_Integer);
	public static readonly  ExpressionType Double= new ExpressionType("double", STD_Double);
	public static readonly  ExpressionType String= new ExpressionType("string", STD_String);
    public static readonly ExpressionType Null = new ExpressionType("null", STD_Null);
    public static readonly ExpressionType Table = new ExpressionType("table", STD_TABLE);
    public static readonly ExpressionType Custom = new ExpressionType("Custom", STD_TABLE);

        String name;
	private int stdType;
	
	/**
	 * @param name
	 */
	public ExpressionType(String name, int stdType) {
		this.name = name;
		this.stdType = stdType;
	}

	public String getName() {
		return name;
	}
	
	public static ExpressionType match(ExpressionType t1, ExpressionType t2){
		if(t1.stdType == t2.stdType)
			return t1;
		
		else if(t1.stdType != STD_Null && t2.stdType==STD_Null){
			return t1;
		}
		
		else if(t2.stdType != STD_Null && t1.stdType==STD_Null){
			return t2;
		}
		
		return null;
	}
	
	
    }
}
