using System;
using ServiceStack.Redis;

namespace Worker
{
    class Program
    {
        static void Main()
        {
            var redisHost = Environment.GetEnvironmentVariable("REDIS_HOST");
            var redisPort = Environment.GetEnvironmentVariable("REDIS_PORT");
            var redis = new RedisManagerPool($"{redisHost}:{redisPort}");
            using (var redisConsumer  = redis.GetClient())
            using (var subscription = redisConsumer.CreateSubscription())
            {
                subscription.OnMessage = (channel, msg) =>
                {
                    const string setKey = "values";
                    var redisKey = setKey + msg;                    
                    using (var redisClient = redis.GetClient())
                    {
                        redisClient.SetValue(redisKey, Fib(Convert.ToInt32(msg)).ToString());
                        redisClient.AddItemToSet(setKey, redisKey);
                    }                    
                };    
                subscription.SubscribeToChannels("message");          
            }
        }

        private static int Fib(int index) 
        {
            if (index < 2) return 1;
            return Fib(index - 1) + Fib(index - 2);         
        }
    }

}
