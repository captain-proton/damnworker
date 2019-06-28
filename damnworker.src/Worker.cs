using System;
using System.Threading;

namespace DamnWorker.src
{

    /*
    Der Delegat ist hier auf Namespacelevel definiert, da es in einem Interface
    nicht erlaubt ist und auf eine harte Kopplung (Alice.WorkDone += ...)
    verzichtet werden soll. Im Gegensatz zum Erklaerungsvideo wird hier
    auf "object sender, EventArgs args" verzichtet. Es genuegt die
    Uebergabe des Workers und der Anzahl an erledigten Arbeiten. Fuer unseren
    Fall wird das sehr wahrscheinlich InputAdapter und MocapFrame sein.
    Der Server bekommt mitgeteilt, dass ein InputAdapter einen MocapFrame
    uebertragen will (Der widerum von eigentlichen System kommt).
     */
    public delegate void WorkDoneEventHandler(IWorker worker, int amount);

    public interface IWorker
    {
        /*
        Jeder Arbeiter enthaelt diesen Event, so dass sich der Boss bei
        jedem Arbeiter darauf anmelden kann. Sobald der Arbeiter gestartet
        wurde werden die Events verteilt (siehe Start() -> OnWorkDone).
         */
        event WorkDoneEventHandler WorkDone;

        string Name { get; set; }

        /*
        Der `CancellationToken` wird verwendet um den Arbeiter anzuhalten.
        Sollte dieser niemals gesetzt werden, beendet der Arbeiter seine
        Arbeit nicht. Da der Boss die Kontrolle der Arbeit steuert ist
        dort auch die Quelle fuer den Token vorhanden 
        (`CancellationTokenSource`).
         */
        CancellationToken CancelToken { get; set; }

        void Run();
    }
    public class Alice : IWorker
    {

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public CancellationToken CancelToken { get; set; }

        public string Name { get; set; }

        public event WorkDoneEventHandler WorkDone;

        protected int WorkTime { get; set; }

        private int amount;

        public Alice()
        {
            this.WorkTime = 1000;
            this.Name = "Alice";
        }

        public void Run()
        {
            amount = 0;
            logger.Info("{0} starting work...", Name);
            while (!CancelToken.IsCancellationRequested)
            {
                Thread.Sleep(WorkTime);
                amount += 1;

                /*
                Wie im Erklaerungsvideo bereits erlaeutert kommt hier
                eine dotnet Empfehlung zum Tragen. Arbeit wurde erledigt
                und wird an alle Subscriber uebertragen. 
                 */
                OnWorkDone();
            }
            logger.Info("... {0} stopped work", Name);
        }

        protected virtual void OnWorkDone()
        {
            /*
            Wenn sich keine angemeldet hat wird auch nichts uebertragen.
            Wichtig ist, dass hier der Boss nicht verwendet wird. Der
            Arbeiter weiss nichts vom Boss, muss es aber auch nicht. Nur
            die am Ereignis interessierten Instanzen sind relevant.
             */
            if (WorkDone != null)
            {

                /* 
                Es werden alle Interessenten adressiert! 
                -> Boss.cs -> Start()
                 */
                WorkDone(this, amount);
            }
        }
    }

    public class Bob : Alice
    {
        public Bob()
        {
            this.WorkTime = 1500;
            this.Name = "Bob";
        }
    }

    public class Sam : Alice
    {
        public Sam()
        {
            this.WorkTime = 4000;
            this.Name = "Sam";
        }
    }
}