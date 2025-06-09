using Neo4j.Driver;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PartnerDataManagement.Web
{

    public class Neo4jService
    {
        private readonly IDriver _driver;

        public Neo4jService(string uri, string user, string password)
        {
            _driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
        }

        public async Task<List<string>> GetSubsidiaries(string parentCompany)
        {
            var session = _driver.AsyncSession();
            var result = await session.RunAsync($"MATCH (p:Company {{name: '{parentCompany}'}})-[:OWNS]->(c) RETURN c.name");

            var subsidiaries = await result.ToListAsync(r => r["c.name"].As<string>());
            return subsidiaries;
        }
    }
}
