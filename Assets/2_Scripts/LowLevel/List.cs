namespace List
{
    /* Enum: index
     * Purpose: Contains the index of all the items in the game
     * Author: Jonathan Karcher
     */
    enum index : byte
    {
        player,
        motherShip,
        motherShipDamaged,
        motherShipExplosion,
        motherShipDeralict,
        alliedShip,
        alliedShipDamaged,
        alliedShipExplosion,
        alliedDeralict,
        enemyBasic,
        enemyBasicDamaged,
        enemyBasicExplosion,
        enemyDeralict,
        enemyFlagship,
        enemyFlagshipDamaged,
        enemyFlagshipExplosion,
        enemyFlagshipDeralict,
        Planet1,
        Planet2,
        Planet3,
        Planet4,
        Planet5,
        Planet6,
        Planet7,
        moon1,
        moon2,
        moon3,
        sun1,
        sun2,
        sun3,
        asteroid,
        commet,
        rocket
    }
    /* Enum: story
     * Purpose: Contains the index of all the story items in the game
     * Author: Jonathan Karcher
     */
    enum story : byte
    {
        tutorial,
        firstContact,
        crabsAttackSol,
        fightAroundSaturn,
        fightAtTheAsteroidBelt,
        fightBetweenEarthAndMars,
        secondFightAtTheAsteroidBelt,
        fightAtPluto,
        fightAtLxion,
        fightAtChrion
    }
}