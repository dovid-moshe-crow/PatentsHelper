using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Utf8Json;

namespace PatentsHelperSettings
{
    public class CasesData : IList<CaseData>
    {
        private static readonly string casesDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PatentsHelper");
        private static readonly string casesDataFilePath = Path.Combine(casesDataPath, "CasesData.json");
        public CasesData()
        {
            try
            {
                using (FileStream fs = File.OpenRead(casesDataFilePath))
                {
                    casesData = JsonSerializer.DeserializeAsync<List<CaseData>>(fs).Result;
                }
            }
            catch
            {
                casesData = new List<CaseData>();
            }
        }

        public void Save()
        {
            Directory.CreateDirectory(casesDataPath);
            using (FileStream fs = File.Create(casesDataFilePath))
            {
                JsonSerializer.SerializeAsync(fs, casesData).Wait();
            }
        }

        private readonly List<CaseData> casesData = new List<CaseData>();


        public bool CaseExists(string name) => GetCaseByName(name) != null;
        public bool LocationExists(string location) => GetCaseByLocation(location) != null;


        public bool LocationExistsInDiffrentCase(string name, string location)
        {
            return casesData.Find(c => !c.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && c.Location?.Equals(location) == true) != null;
        }

        public CaseData GetCaseByLocation(string location)
        {
            return casesData.Find(c => c.Location?.Equals(location) == true);
        }

        public CaseData GetCaseByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }
            return casesData.Find(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public void AddOrUpdateCase(string name, string location)
        {
            var caseData = GetCaseByName(name);
            if (caseData == null)
            {
                caseData = new CaseData { DateCreated = DateTime.Now, LastTimeAccessed = DateTime.Now, Location = location, Name = name };
                Add(caseData);
            }
            else
            {
                caseData.Location = location;
                caseData.LastTimeAccessed = DateTime.Now;
            }
            Save();
        }

        public void LastTimeUsedCase(string caseName)
        {
            var caseData = GetCaseByName(caseName);
            if (caseData == null)
            {
                Add(new CaseData { DateCreated = DateTime.Now, LastTimeAccessed = DateTime.Now, Name = caseName });
            }
            else
            {
                caseData.LastTimeAccessed = DateTime.Now;
            }
            Save();
        }

        public void LastTimeUsedCaseByLocation(string location)
        {
            var caseDate = GetCaseByLocation(location);

            if (caseDate != null)
            {
                caseDate.LastTimeAccessed = DateTime.Now;
                Save();
            }
        }

        public CaseData this[int index] { get => casesData[index]; set => casesData[index] = value; }

        public int Count => casesData.Count;

        public bool IsReadOnly => false;

        public void Add(CaseData item)
        {
            casesData.Add(item);
        }

        public void Clear()
        {
            casesData.Clear();
        }

        public bool Contains(CaseData item)
        {
            return casesData.Contains(item);
        }

        public void CopyTo(CaseData[] array, int arrayIndex)
        {
            casesData.CopyTo(array, arrayIndex);
        }

        public IEnumerator<CaseData> GetEnumerator()
        {
            return casesData.GetEnumerator();
        }

        public int IndexOf(CaseData item)
        {
            return casesData.IndexOf(item);
        }

        public void Insert(int index, CaseData item)
        {
            casesData.Insert(index, item);
        }

        public bool Remove(CaseData item)
        {
            return casesData.Remove(item);
        }

        public void RemoveAt(int index)
        {
            casesData.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return casesData.GetEnumerator();
        }
    }

    public class CaseData
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastTimeAccessed { get; set; }

    }
}
