using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dashing.net.common
{
    public interface IDataRetriever
    {
        /// <summary>Retuns a string array of the Widget Ids this processor is capable of processing.</summary>
        /// <value>The processor widget ids.</value>
        string[] ProcessorWidgetIds { get; }

        // gets your json data to send back to the user
        object GetData(string widgetId);
    }
}
