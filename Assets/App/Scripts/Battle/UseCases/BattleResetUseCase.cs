using App.Battle.Interfaces.DataStores;
using App.Battle.Interfaces.UseCases;

namespace App.Battle.UseCases
{
    public class BattleResetUseCase : IBattleResetUseCase
    {
        private readonly IPlayerHandDataStore _playerHandDataStore;
        private readonly IPlayerStageAreaDataStore _playerStageAreaDataStore;
        private readonly IPlayerTrashDataStore _playerTrashAreaDataStore;
        private readonly IPlayerBreakAreaDataStore _playerBreakAreaDataStore;
        private readonly IPlayerSupportAreaDataStore _playerSupportAreaDataStore;
        private readonly IPlayerBattleAreaCookieDataStore _playerBattleAreaCookieDataStore;
        private readonly IPlayerBattleAreaCookieHpDataStore _playerBattleAreaCookieHpDataStore;
        private readonly IPlayerDeckDataStore _playerDeckDataStore;

        public BattleResetUseCase(
            IPlayerHandDataStore playerHandDataStore,
            IPlayerStageAreaDataStore playerStageAreaDataStore,
            IPlayerTrashDataStore playerTrashDataStore,
            IPlayerBreakAreaDataStore playerBreakAreaDataStore,
            IPlayerSupportAreaDataStore playerSupportAreaDataStore,
            IPlayerBattleAreaCookieDataStore playerBattleAreaCookieDataStore,
            IPlayerBattleAreaCookieHpDataStore playerBattleAreaCookieHpDataStore,
            IPlayerDeckDataStore playerDeckDataStore
        )
        {
            _playerHandDataStore = playerHandDataStore;
            _playerStageAreaDataStore = playerStageAreaDataStore;
            _playerTrashAreaDataStore = playerTrashDataStore;
            _playerBreakAreaDataStore = playerBreakAreaDataStore;
            _playerSupportAreaDataStore = playerSupportAreaDataStore;
            _playerBattleAreaCookieDataStore = playerBattleAreaCookieDataStore;
            _playerBattleAreaCookieHpDataStore = playerBattleAreaCookieHpDataStore;
            _playerDeckDataStore = playerDeckDataStore;
        }

        public void Execute()
        {
            _playerHandDataStore.Clear();
            _playerStageAreaDataStore.RemoveCard();
            _playerTrashAreaDataStore.Clear();
            _playerBreakAreaDataStore.Clear();
            _playerSupportAreaDataStore.Clear();
            _playerBattleAreaCookieDataStore.Clear();
            _playerBattleAreaCookieHpDataStore.Clear();
            _playerDeckDataStore.ClearOf("player1");
        }
    }
}