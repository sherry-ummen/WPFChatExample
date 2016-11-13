namespace Server {
    public static class Configuration{
        public const string HOCONConfigLocalhost = @"
akka {  
    actor {
        provider = ""Akka.Remote.RemoteActorRefProvider, Akka.Remote""
    }
    remote {
        helios.tcp {
            port = 8081
            hostname = 0.0.0.0
            public-hostname = localhost
        }
    }
}
";
        public const string HOCONConfigServer = @"
akka {  
    actor {
        provider = ""Akka.Remote.RemoteActorRefProvider, Akka.Remote""
    }
    remote {
        helios.tcp {
            port = 8081
            hostname = 10.181.2.3
            public-hostname = serverdbtesting.paand.local
        }
    }
}
";
    }
}
