// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataProcessor.cs" company="iCareHealth (UK) Ltd">
//   Copyright (c) iCareHealth (UK) Ltd. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace dashing.net.common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public interface IDataProcessor
    {
        /// <summary>Processes the data.</summary>
        /// <param name="data">The data.</param>
        void ProcessData(string widgetId, IDictionary<string, object> data);

        /// <summary>Retuns a string array of the Widget Ids this processor is capable of processing.</summary>
        /// <value>The processor widget ids.</value>
        string[] ProcessorWidgetIds { get; }
    }
}
