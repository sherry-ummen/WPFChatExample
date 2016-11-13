namespace ChatExample {
    internal static class Configuration{
        public static string ConnnectionString = "akka.tcp://MyServer@serverdbtesting.paand.local:8081/user/ChatServer";
        public static string HOCONConfig = @"
akka {  
    actor {
        provider = ""Akka.Remote.RemoteActorRefProvider, Akka.Remote""
    }
    remote {
        helios.tcp {
		    port = 0
		    hostname = localhost
        }
    }
}
";
    }
}
