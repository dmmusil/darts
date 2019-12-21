using Projac.Sql;
using Projac.SqlClient;

namespace Darts.QueueProcessing
{
    public class UserRegistration : SqlProjection
    {
        private static readonly SqlClientSyntax Sql = new SqlClientSyntax();
        public UserRegistration()
        {
            When<RegistrationRequested>(
                e => Sql.NonQueryStatement(
                    "insert into UserProfiles (Username, Email) values (@P1, @P2)",
                    new { P1 = e.Username, P2 = e.Email }));

            When
        }


    }


}
