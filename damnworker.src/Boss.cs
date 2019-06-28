using System.Threading;

namespace DamnWorker.src
{
    public interface IBoss
    {
        void Start();

        void Stop();
    }

    public class Boss : IBoss
    {

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        readonly IWorker[] allWorkers;

        /*
        Zur Beendung der Arbeiter wird eine Spezialitaet von dotnet verwendet,
        der `CancellationTokenSource` bzw `CancellationToken`. Einigermaszen
        vernuenftige Doku gibt es bei Microsoft.

        https://docs.microsoft.com/en-us/dotnet/standard/threading/cancellation-in-managed-threads

        Vorteil ist, dass jeder Thread auf diese eine Instanz des Tokens
        zurueckgreifen und weitergeben kann. Es muss nur an einer Stelle
        die Anfrage auf das Anhalten, hier im Boss, gestellt werden. Das
        ist ein wenig sicherer als Boolsche Werte zu setzen. Sollte die
        Arbeit jedes Arbeiters einzeln beendet werden empfiehlt es sich
        die Quelle in den Arbeiter zu verlagern und die Token nach Aussen
        zur Verfuegung zu stellen.
         */
        readonly CancellationTokenSource cancelTokenSource;

        /*
        Der Dependency Injektor kennt den Boss und die Arbeiter. Unter
        anderem durch die Verwendung des Konstruktors koennen alle Klassen
        die als IWorker durch das Binding festgelegt wurden bei der 
        Instanziierung des Boss uebergeben werden. So hat der Boss jeweils
        eine Instanz zu Alice, Bob und Sam ohne dass diese hart eingebunden
        wurden.

        Damit ist DI erst mal abgeschlossen, weiter gehts in der README.
         */
        public Boss(IWorker[] allWorkers)
        {
            this.allWorkers = allWorkers;
            this.cancelTokenSource = new CancellationTokenSource();
        }

        public void Start()
        {
            /*
            Nach Start des Boss werden alle Arbeiter gestartet. Der Boss
            meldet sich auch gleich als Subscriber an. OnWorkDone
            wird aufgerufen, wenn ein Arbeiter das Ereignis ausloest.
            Die Signatur von OnWorkDone stimmt mit dem Delegaten in
            Worker.cs ueberein.

            Weiter gehts mit Threads in der README.
             */
            foreach (var worker in allWorkers)
            {
                worker.WorkDone += OnWorkDone;
                worker.CancelToken = cancelTokenSource.Token;

                /*
                Einfache Erzeugung eines neuen Threads. Durch t.Start()
                wird dieser ausgefuehrt. Hier wird lediglich die Methode
                Run des Arbeiters uebergeben, die dann in dem Thread
                ausgefuehrt wird. Wichtig vor allem fuer eine UI ist, dass
                alle Ereignisse, die durch den Arbeiter innerhalb der
                Methode ausgeloest werden auch in dem Thread statt finden
                und nicht in dem Thread in dem der Arbeiter gestartet wurde.

                -> Boss.Stop()
                 */
                Thread t = new Thread(worker.Run);
                t.Start();
            }
        }

        public void Stop()
        {
            /*
            Die Benahmung Stop hat NICHTS! mit dem Thread zu tun. Weder
            im Boss noch in Worker.cs. Im Arbeiter wird lediglich eine Property
            gesetzt, die regelmaeszig abgefragt wird und die Ausfuehrung
            beendet. Nach Abmeldung des EventHandler Instanz wird der
            Arbeiter ueber den CancelToken gestoppt und das Programm haelt an.

            -> Boss.OnWorkDone()
             */
            cancelTokenSource.Cancel();
            foreach (var worker in allWorkers)
            {
                worker.WorkDone -= OnWorkDone;
            }
        }

        public void OnWorkDone(IWorker worker, int amount)
        {
            /*
            Das ist nur eine einfache Ausgabe! Wichtig ist aber die erzeugte
            Ausgabe. In dem Logger wird der Thread mit ausgegeben in dem
            das Ereignis ausgeloest wurde. Sollte irgendwann eine UI
            adressiert werden muss zu dem UI-Thread zurueckgekehrt werden,
            da ansonsten die Anwendung abstuerzt!

            Zurueck zur README
             */
            logger.Info("{0,10} at {1}", worker.Name, amount);
        }
    }
}