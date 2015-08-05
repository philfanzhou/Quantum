using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DistributedServices.WCF
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TokenRequiredAttribute : Attribute
    {
    }
}
