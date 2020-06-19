using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Expressions
{
    public static class DatabaseUtils
    {
        public static void appendEscapedSQLString(this StringBuilder sb, string sqlString)
        {
            sb.Append('\'');
            if (sqlString.IndexOf('\'') != -1)
            {
                int length = sqlString.Length;
                for (int i = 0; i < length; i++)
                {
                    char c = sqlString[i];
                    if (c == '\'')
                    {
                        sb.Append('\'');
                    }
                    sb.Append(c);
                }
            }
            else
                sb.Append(sqlString);
            sb.Append('\'');
        }
    }
}
