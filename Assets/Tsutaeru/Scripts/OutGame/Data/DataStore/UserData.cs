using System.Collections.Generic;
using Newtonsoft.Json;
using PlayFab.ClientModels;
using Tsutaeru.OutGame.Data.Entity;

namespace Tsutaeru.OutGame.Data.DataStore
{
    public sealed class UserData
    {
        private readonly Dictionary<string, UserDataRecord> _records;
        public readonly UserEntity user;

        public UserData(string name, Dictionary<string, UserDataRecord> records)
        {
            _records = records;

            user = new UserEntity();
            user.SetUserName(name);
            user.SetTimeAttack(GetUserTimeAttack());
        }

        private UserTimeAttackEntity GetUserTimeAttack()
        {
            return _records.TryGetValue(PlayFabConfig.USER_TIME_ATTACK_KEY, out var record)
                ? JsonConvert.DeserializeObject<UserTimeAttackEntity>(record.Value)
                : UserTimeAttackEntity.Default();
        }
    }
}