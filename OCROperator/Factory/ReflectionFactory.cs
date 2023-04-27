using AutoMapper;
using OCROperator.Models.Interface;
using OCROperator.Models.Interface.Action;
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
            TypedWatcher = SetupAction(TypedWatcher);
            return TypedWatcher;
        }

        private IWatcher SetupAction(IWatcher typedWatcher)
        {
            Type ActionType = Type.GetType(typedWatcher.ActionType);
            IAction Typed = (IAction)Activator.CreateInstance(ActionType);
            Typed.Settings = typedWatcher.ActionSettings;
            typedWatcher.Action = Typed;

            return typedWatcher;
        }

        internal IWatcher CopyPropertiesIntoObject(IWatcher SourceWatcher, IWatcher DestinationWatcher)
        {
            MapperConfiguration mapperConfig = new MapperConfiguration(config =>
            {
                config.CreateMap(SourceWatcher.GetType(), DestinationWatcher.GetType());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            DestinationWatcher = (IWatcher)mapper.Map(SourceWatcher, SourceWatcher.GetType(), DestinationWatcher.GetType());
            return DestinationWatcher;
        }
    }
}
