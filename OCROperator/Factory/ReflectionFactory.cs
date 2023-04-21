using OCROperator.Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OCROperator.Factory
{
    internal class ReflectionFactory
    {
        internal IWatcher CreateObjectFromRaw(RawWatcher watcher)
        {
            Type WatcherType = Type.GetType(watcher.Type);
            IWatcher TypedWatcher = (IWatcher)Activator.CreateInstance(WatcherType);

            TypedWatcher = CopyPropertiesIntoObject(watcher, TypedWatcher);
            return TypedWatcher;
        }

        internal IWatcher CopyPropertiesIntoObject(IWatcher SourceWatcher, IWatcher DestinationWatcher)
        {
            DestinationWatcher.Destination = SourceWatcher.Destination;
            DestinationWatcher.SuffixMetadata = SourceWatcher.SuffixMetadata;
            DestinationWatcher.Action = SourceWatcher.Action;
            DestinationWatcher.Type = SourceWatcher.Type;
            DestinationWatcher.Language = SourceWatcher.Language;
            return DestinationWatcher;
        }
    }
}
