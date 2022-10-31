using SqlKata;
using SqlKata.Compilers;

namespace TheatersOfTheCity.Data.Helpers;

public static class SqlToStringService
{
    public static string MySqlQueryToString(this Query query)
    {
        var compiler = new MySqlCompiler();
        SqlResult sqlResult = compiler.Compile(query);
        return sqlResult.ToString();
    }
}