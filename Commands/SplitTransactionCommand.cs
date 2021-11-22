using System.Collections.Generic;

namespace PFM.Commands
{
    public class SplitTransactionCommand
    {
        public SplitTransactionCommand(){}
        public List<SplitTransactionModel> splits { get; set; }
    }
}