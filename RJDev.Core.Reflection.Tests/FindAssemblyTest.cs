using System.Collections.Generic;
using System.Linq;
using RJDev.Core.Reflection.AssemblyFinder;
using Xunit;
using Xunit.Abstractions;

namespace RJDev.Core.Reflection.Tests
{
    public class FindAssemblyTest
    {
        private readonly ITestOutputHelper testOutputHelper;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="testOutputHelper"></param>
        public FindAssemblyTest(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void FindAssembly()
        {
            IAssemblyFinder af = new DefaultAssemblyFinder();
            IEnumerable<string> assemblies = af.GetAssemblies("RJDev.*")
                .Select(x => x.GetName().Name)
                .OrderBy(x => x);

            Assert.Collection(
                assemblies,
                name => Assert.Equal("RJDev.Core.Reflection", name),
                name => Assert.Equal("RJDev.Core.Reflection.Tests", name)
            );
        }
    }
}