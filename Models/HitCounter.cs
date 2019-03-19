using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SportStoreCore.Models
{
    public interface IHitCounterService
    {
        int UpdateCount();
    }
    public class HitCounter : IHitCounterService
    {
        private string _rootPath;
        public HitCounter(string rootPath)
        {
            _rootPath = rootPath;
        }
        public int UpdateCount()
        {
            var hitCountFilePath = "D:\\SportStoreCore\\Models\\hitcount.txt";
            var fullPath = string.Concat(_rootPath, hitCountFilePath);
            var count = int.Parse(File.ReadAllText(fullPath));
            count++;
            File.WriteAllText(fullPath, count.ToString());
            return count;
        }
    }
}