using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.User
{
    public class GetCachedUsers
    {
        public class Query : IRequest<List<UserCacheDto>>
        {

        }

        public class Handler : IRequestHandler<Query, List<UserCacheDto>>
        {

            private IDistributedCache _cache;
            private IHostApplicationLifetime _lifetime;
            public Handler(IDistributedCache cache, IHostApplicationLifetime lifetime)
            {
                _cache = cache;
                _lifetime = lifetime;
            }
            public async Task<List<UserCacheDto>> Handle(Query request, CancellationToken cancellationToken)
            {

                byte[] cachedUsers = await _cache.GetAsync("cachedUsers");

                if (cachedUsers != null)
                {
                    var usersstring = Encoding.UTF8.GetString(cachedUsers);
                    var users = JsonConvert.DeserializeObject<List<UserCacheDto>>(usersstring);
                    return users;
                }

                return null;
            }
        }
    }
}
