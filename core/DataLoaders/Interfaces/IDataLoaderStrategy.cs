using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.DataLoaders.Interfaces
{
    public interface IDataLoaderStrategy
    {
        /// <summary>
        /// Loads plain test data from the specified file.
        /// Suitable for simple data formats like arrays of strings or numbers.
        /// </summary>
        /// <param name="filePath">The path to the file containing the test data.</param>
        /// <returns>An enumerable of object arrays representing the test data.</returns>
        IEnumerable<object[]> LoadData(string filePath);

        /// <summary>
        /// Loads test data from the specified file and maps it to the specified type.
        /// Suitable for complex data formats that require deserialization into specific types.
        /// </summary>
        /// <param name="filePath">The path to the file containing the test data.</param>
        /// <param name="type">The type to which the test data should be mapped.</param>
        /// <returns>An enumerable of object arrays representing the test data.</returns>
        IEnumerable<object[]> LoadData(string filePath, Type type);
    }
}
