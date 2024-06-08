public static class Constants
{
    public const int LAYER_PLAYER_UNIT = 6;
    public const int LAYER_ENEMY_UNIT = 7;


    public const int MASK_PLAYER_UNIT = 1 << LAYER_PLAYER_UNIT;
    public const int MASK_ENEMY_UNIT = 1 << LAYER_ENEMY_UNIT;
    public const int MASK_BATTLE_UNIT = MASK_PLAYER_UNIT | MASK_ENEMY_UNIT;

    public const bool ENABLE_MOVEMENT_PATH_DEBUGGING = true;
}
