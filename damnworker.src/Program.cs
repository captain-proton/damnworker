using System;
using Ninject;

namespace DamnWorker.src {
    class Program {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger ();
        
        static void Main (string[] args) {
            /*
            Erstellt einen neuen Kernel der in der Lage ist neue Objekte
            zur Erzeugen oder zu holen. Zu diesem Zweck wird ein Modul
            benoetigt, dass sogenannte Bindings enthaelt. Es findet die 
            Bindung von konkreten Implementierungen an Interfaces statt.
             */
            Ninject.IKernel kernel = new StandardKernel(new DamnModule());

            /*
            Z.B. ist der Boss verantwortlich fuer die Ausfuehrung von Arbeit.
            Fuer unseren Fall waere das Equivalent der "MainAppServer".
            Durch den Kernel kommen wir an eine Instanz. Wie der genau aussieht
            kann aus voellig egal sein. Es werden an dieser Stelle nur die
            Methoden den Interface benoetigt.
             */
            var boss = kernel.Get<IBoss>();

            // .... fang an zu arbeiten!
            boss.Start();

            /*
            Diese Zeile ist nur dazu da um zu warten bis eine Taste auf
            dem Keyboard gedrueckt wurde, damit das Programm abbricht.
             */
            Console.ReadKey();

            /* Feierabend fuer den Boss und seine Arbeiter */
            boss.Stop();

            /*
            Wenn nicht bereits geschehen lest im DamnModule weiter, dann
            ist das Thema der Dependecy Injection abgeschlossen.
             */
        }
    }
}