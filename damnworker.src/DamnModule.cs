namespace DamnWorker.src {
    public class DamnModule : Ninject.Modules.NinjectModule 
    {
        public override void Load () 
        {

            /*
            Die Methode Load ist durch das NinjectModule vorgegeben. Hier
            werden Interfaces an konkrete Implementierungen gebunden. Das
            ist die einzige Stelle an der eine harte Bindung erfolgt. Man
            koennte hier auch mit Reflections arbeiten, was die ganze Sache
            zur Erklaerung aber nur verkomplizieren wuerde.

            Durch die Ausfuehrung von Bind wird ein Interface an eine
            Klasse gebunden. Der im Programm erstellte Kernel kann dann
            mit Hilfe des Moduls die Instanzen zu den Klassen erzeugen.
            Auf sowas wie "new Alice()" etc. kann verzichtet werden.

            Hier kommt auch direkt eine Besonderheit zum Tragen, die Multi
            Injection:

            https://github.com/ninject/Ninject/wiki/Multi-injection

            IWorker
                -> Alice
                -> Bob
                -> Sam

            Dem Boss/Server ist egal wer arbeitet, hauptsache es wird
            gearbeitet.
             */
            Bind<IBoss>().To<Boss>();
            Bind<IWorker>().To<Alice>();
            Bind<IWorker>().To<Bob>();
            Bind<IWorker>().To<Sam>();

            /*
            Irgendwie muss der Boss aber noch an seine Arbeiter kommen, daher
            gehts im Konstruktor von Boss.cs weiter.
             */
        }
    }
}