using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

 namespace DiagnosisProjects.Safari
{
    public interface IPrefixMatcher<V> where V : class
    {
        String GetPrefix();
        void ResetMatch();
        void BackMatch();
        char LastMatch();
        bool NextMatch(char next);
        List<V> GetPrefixMatches();
        bool IsExactMatch();
        V GetExactMatch();
    }
}
