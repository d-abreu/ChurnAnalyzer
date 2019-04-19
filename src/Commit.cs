using System;
using System.Collections.Generic;

namespace ChurnAnalyzers
{
    public class Commit
    {
        public Commit(DateTime date, IEnumerable<FileChanged> changes)
        {
            Date = date;
            foreach (var item in changes)
            {
                FileInfos.AddLast(item);
            }
        }
        public DateTime Date {get;}
        public LinkedList<FileChanged> FileInfos {get;} = new LinkedList<FileChanged>();
        public class FileChanged
        {
            public FileChanged(string name, int added, int removed)
            {
                FileName = name;
                Changed = Math.Abs(added - removed);
                Added = added - Changed;
                Removed = removed - Changed;
            }
            public string FileName {get;}
            public int Added {get;}
            public int Removed {get;}
            public int Changed {get;}
        }
    }
}