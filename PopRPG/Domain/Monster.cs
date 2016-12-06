using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    [Serializable]
    public class Monster : Fighter
    {
        public Monster(SerializationInfo info, StreamingContext context)
        {

        }
        public Monster()
        {

        }
    }
}
