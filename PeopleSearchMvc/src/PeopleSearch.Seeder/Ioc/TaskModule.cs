using System.Threading.Tasks;
using Ninject.Modules;

namespace PeopleSearch.Seeder.Ioc
{
    // todo: this needs to be pulled out in to it's own shareable library--potentially a nuget package
    public class TaskModule : NinjectModule
    {
        public override void Load()
        {
            Bind<TaskFactory>().ToSelf();
        }
    }
}