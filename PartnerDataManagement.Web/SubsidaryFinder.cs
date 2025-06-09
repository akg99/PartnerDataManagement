using Neo4j.Driver;

namespace PartnerDataManagement.Web
{
    public class SubsidiaryFinder
    {
        private readonly IDriver _driver;

        public SubsidiaryFinder()
        {
            _driver = GraphDatabase.Driver("bolt://localhost:7687", AuthTokens.Basic("neo4j", "password"));
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
