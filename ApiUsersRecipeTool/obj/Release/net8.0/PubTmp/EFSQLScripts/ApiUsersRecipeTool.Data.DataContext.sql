CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) NOT NULL,
    `ProductVersion` varchar(32) NOT NULL,
    PRIMARY KEY (`MigrationId`)
);

START TRANSACTION;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240515210522_PrimerMigrartion')
BEGIN
    CREATE TABLE `Users` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Username` longtext NOT NULL,
        `Password` longtext NOT NULL,
        PRIMARY KEY (`Id`)
    );
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240515210522_PrimerMigrartion')
BEGIN
    CREATE TABLE `Recipes` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Url` longtext NOT NULL,
        `UserId` int NULL,
        PRIMARY KEY (`Id`),
        CONSTRAINT `FK_Recipes_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`)
    );
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240515210522_PrimerMigrartion')
BEGIN
    CREATE INDEX `IX_Recipes_UserId` ON `Recipes` (`UserId`);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240515210522_PrimerMigrartion')
BEGIN
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20240515210522_PrimerMigrartion', '8.0.5');
END;

COMMIT;

START TRANSACTION;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240528063634_BuyList')
BEGIN
    CREATE TABLE `BuyLists` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Name` longtext NOT NULL,
        PRIMARY KEY (`Id`)
    );
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240528063634_BuyList')
BEGIN
    CREATE TABLE `Ingredients` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Name` longtext NOT NULL,
        `BuyListId` int NULL,
        PRIMARY KEY (`Id`),
        CONSTRAINT `FK_Ingredients_BuyLists_BuyListId` FOREIGN KEY (`BuyListId`) REFERENCES `BuyLists` (`Id`)
    );
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240528063634_BuyList')
BEGIN
    CREATE INDEX `IX_Ingredients_BuyListId` ON `Ingredients` (`BuyListId`);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240528063634_BuyList')
BEGIN
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20240528063634_BuyList', '8.0.5');
END;

COMMIT;

START TRANSACTION;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240529074838_ListBuy')
BEGIN
    ALTER TABLE `Ingredients` DROP CONSTRAINT `FK_Ingredients_BuyLists_BuyListId`;
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240529074838_ListBuy')
BEGIN
    ALTER TABLE `Ingredients` MODIFY `BuyListId` int NOT NULL DEFAULT 0;
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240529074838_ListBuy')
BEGIN
    ALTER TABLE `BuyLists` ADD `UserId` int NULL;
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240529074838_ListBuy')
BEGIN
    CREATE INDEX `IX_BuyLists_UserId` ON `BuyLists` (`UserId`);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240529074838_ListBuy')
BEGIN
    ALTER TABLE `BuyLists` ADD CONSTRAINT `FK_BuyLists_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240529074838_ListBuy')
BEGIN
    ALTER TABLE `Ingredients` ADD CONSTRAINT `FK_Ingredients_BuyLists_BuyListId` FOREIGN KEY (`BuyListId`) REFERENCES `BuyLists` (`Id`) ON DELETE CASCADE;
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240529074838_ListBuy')
BEGIN
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20240529074838_ListBuy', '8.0.5');
END;

COMMIT;

