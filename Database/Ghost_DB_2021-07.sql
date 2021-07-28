SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;


CREATE TABLE `accounts` (
  `id` int(11) NOT NULL,
  `username` varchar(20) CHARACTER SET big5 NOT NULL,
  `password` varchar(128) CHARACTER SET big5 NOT NULL,
  `isTwoFactor` int(1) DEFAULT 0,
  `TwoFactorPassword` varchar(20) DEFAULT NULL,
  `creation` datetime NOT NULL,
  `character_slot` int(2) DEFAULT 4,
  `isLoggedIn` int(1) DEFAULT 0,
  `isBanned` int(1) DEFAULT 0,
  `isMaster` int(1) DEFAULT 0,
  `gamePoints` int(11) DEFAULT 0,
  `giftPoints` int(11) DEFAULT 0,
  `bonusPoints` int(11) DEFAULT 0,
  `total_donate` int(8) DEFAULT 0,
  `VIP` int(1) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC;

CREATE TABLE `amuletcommodity` (
  `id` int(11) NOT NULL,
  `itemID` int(11) NOT NULL,
  `price` int(11) NOT NULL,
  `bargainPrice` int(11) NOT NULL,
  `term` int(11) NOT NULL DEFAULT -1,
  `flag` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC;

CREATE TABLE `boydresscommodity` (
  `id` int(11) NOT NULL,
  `itemID` int(11) NOT NULL,
  `price` int(11) NOT NULL,
  `bargainPrice` int(11) NOT NULL,
  `term` int(11) NOT NULL DEFAULT -1,
  `flag` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC;

CREATE TABLE `boyeyescommodity` (
  `id` int(11) NOT NULL,
  `itemID` int(11) NOT NULL,
  `price` int(11) NOT NULL,
  `bargainPrice` int(11) NOT NULL,
  `term` int(11) NOT NULL DEFAULT -1,
  `flag` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC;

CREATE TABLE `boyhaircommodity` (
  `id` int(11) NOT NULL,
  `itemID` int(11) NOT NULL,
  `price` int(11) NOT NULL,
  `bargainPrice` int(11) NOT NULL,
  `term` int(11) NOT NULL DEFAULT -1,
  `flag` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC;

CREATE TABLE `buycommoditylog` (
  `id` int(11) NOT NULL,
  `name` varchar(20) CHARACTER SET big5 NOT NULL,
  `itemID` int(11) NOT NULL,
  `itemName` varchar(62) CHARACTER SET big5 NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC;

CREATE TABLE `characters` (
  `id` int(11) NOT NULL,
  `accountId` int(11) NOT NULL,
  `worldId` int(1) NOT NULL,
  `name` varchar(20) CHARACTER SET utf8 COLLATE utf8_thai_520_w2 NOT NULL,
  `title` varchar(20) CHARACTER SET big5 NOT NULL,
  `gender` int(1) NOT NULL DEFAULT 1,
  `hair` int(8) NOT NULL,
  `eyes` int(8) NOT NULL,
  `skin` int(10) DEFAULT 0,
  `level` int(4) NOT NULL DEFAULT 1,
  `job` int(4) DEFAULT 0,
  `classId` int(4) NOT NULL DEFAULT 0,
  `classLv` int(4) NOT NULL DEFAULT -1,
  `guild` int(4) NOT NULL,
  `hp` int(8) NOT NULL DEFAULT 1,
  `maxHp` int(8) NOT NULL DEFAULT 1,
  `mp` int(8) NOT NULL DEFAULT 1,
  `maxMp` int(8) NOT NULL DEFAULT 1,
  `fury` int(8) NOT NULL DEFAULT 0,
  `maxFury` int(8) NOT NULL DEFAULT 1200,
  `exp` int(8) NOT NULL DEFAULT 0,
  `rank` int(8) NOT NULL DEFAULT 0,
  `money` int(8) NOT NULL DEFAULT 0,
  `c_str` int(4) NOT NULL DEFAULT 3,
  `c_dex` int(4) NOT NULL DEFAULT 3,
  `c_vit` int(4) NOT NULL DEFAULT 3,
  `c_int` int(4) NOT NULL DEFAULT 3,
  `upgradeStr` int(4) NOT NULL DEFAULT 0,
  `upgradeDex` int(4) NOT NULL DEFAULT 0,
  `upgradeVit` int(4) NOT NULL DEFAULT 0,
  `upgradeInt` int(4) NOT NULL DEFAULT 0,
  `attack` int(4) NOT NULL DEFAULT 0,
  `maxAttack` int(4) NOT NULL DEFAULT 0,
  `upgradeAttack` int(4) NOT NULL DEFAULT 0,
  `magic` int(4) NOT NULL DEFAULT 0,
  `maxMagic` int(4) NOT NULL DEFAULT 0,
  `upgradeMagic` int(4) NOT NULL DEFAULT 0,
  `avoid` int(4) NOT NULL DEFAULT 0,
  `defense` int(4) NOT NULL DEFAULT 0,
  `upgradeDefense` int(4) NOT NULL DEFAULT 0,
  `abilityBonus` int(4) NOT NULL DEFAULT 0,
  `skillBonus` int(4) NOT NULL DEFAULT 0,
  `jumpHeight` int(4) NOT NULL DEFAULT 3,
  `mapX` int(4) NOT NULL DEFAULT 0,
  `mapY` int(4) NOT NULL DEFAULT 0,
  `playerX` int(4) NOT NULL DEFAULT 0,
  `playerY` int(4) NOT NULL DEFAULT 0,
  `position` int(4) NOT NULL DEFAULT -1,
  `isDeleted` int(2) DEFAULT 0,
  `deleted_at` varchar(255) DEFAULT NULL,
  `created_at` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC;

CREATE TABLE `coupon` (
  `id` int(11) NOT NULL,
  `code` varchar(20) NOT NULL,
  `itemID` int(8) NOT NULL,
  `quantity` int(3) NOT NULL,
  `valid` int(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC;

CREATE TABLE `drop_data` (
  `id` int(11) NOT NULL,
  `mobID` int(11) NOT NULL DEFAULT 0,
  `itemID` int(11) NOT NULL DEFAULT 0,
  `min_quantity` int(11) NOT NULL DEFAULT 1,
  `max_quantity` int(11) NOT NULL DEFAULT 1,
  `questID` int(11) NOT NULL DEFAULT 0,
  `chance` int(11) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC;

CREATE TABLE `face1commodity` (
  `id` int(11) NOT NULL,
  `itemID` int(11) NOT NULL,
  `price` int(11) NOT NULL,
  `bargainPrice` int(11) NOT NULL,
  `term` int(11) NOT NULL DEFAULT -1,
  `flag` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC;

CREATE TABLE `face2commodity` (
  `id` int(11) NOT NULL,
  `itemID` int(11) NOT NULL,
  `price` int(11) NOT NULL,
  `bargainPrice` int(11) NOT NULL,
  `term` int(11) NOT NULL DEFAULT -1,
  `flag` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC;

CREATE TABLE `gifts` (
  `id` int(11) NOT NULL,
  `name` varchar(20) CHARACTER SET big5 NOT NULL,
  `itemID` int(11) NOT NULL,
  `itemName` varchar(62) CHARACTER SET big5 NOT NULL,
  `receive` int(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC;

CREATE TABLE `girldresscommodity` (
  `id` int(11) NOT NULL,
  `itemID` int(11) NOT NULL,
  `price` int(11) NOT NULL,
  `bargainPrice` int(11) NOT NULL,
  `term` int(11) NOT NULL DEFAULT -1,
  `flag` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC;

CREATE TABLE `girleyescommodity` (
  `id` int(11) NOT NULL,
  `itemID` int(11) NOT NULL,
  `price` int(11) NOT NULL,
  `bargainPrice` int(11) NOT NULL,
  `term` int(11) NOT NULL DEFAULT -1,
  `flag` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC;

CREATE TABLE `girlhaircommodity` (
  `id` int(11) NOT NULL,
  `itemID` int(11) NOT NULL,
  `price` int(11) NOT NULL,
  `bargainPrice` int(11) NOT NULL,
  `term` int(11) NOT NULL DEFAULT -1,
  `flag` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC;

CREATE TABLE `hatcommodity` (
  `id` int(11) NOT NULL,
  `itemID` int(11) NOT NULL,
  `price` int(11) NOT NULL,
  `bargainPrice` int(11) NOT NULL,
  `term` int(11) NOT NULL DEFAULT -1,
  `flag` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC;

CREATE TABLE `items` (
  `id` int(11) NOT NULL,
  `cid` int(11) NOT NULL,
  `itemId` int(8) NOT NULL DEFAULT 8810011,
  `quantity` int(3) NOT NULL,
  `spirit` int(11) NOT NULL DEFAULT -1,
  `level1` int(3) NOT NULL DEFAULT 0,
  `level2` int(3) NOT NULL DEFAULT 0,
  `level3` int(3) NOT NULL DEFAULT 0,
  `level4` int(3) NOT NULL DEFAULT 0,
  `level5` int(3) NOT NULL DEFAULT 0,
  `level6` int(3) NOT NULL DEFAULT 0,
  `level7` int(3) NOT NULL DEFAULT 0,
  `level8` int(3) NOT NULL DEFAULT 0,
  `level9` int(3) NOT NULL DEFAULT 0,
  `level10` int(3) NOT NULL DEFAULT 0,
  `fusion` int(3) NOT NULL DEFAULT 0,
  `isLocked` int(1) NOT NULL DEFAULT 0,
  `icon` int(11) NOT NULL,
  `term` int(11) NOT NULL DEFAULT -1,
  `type` int(3) NOT NULL DEFAULT 0,
  `slot` int(3) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC;

CREATE TABLE `keymaps` (
  `cid` int(11) NOT NULL,
  `quickslot` varchar(8) CHARACTER SET big5 NOT NULL,
  `skillID` int(6) NOT NULL,
  `type` int(3) NOT NULL,
  `slot` int(3) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC;

CREATE TABLE `log_create_player` (
  `job` int(2) NOT NULL,
  `nickname` varchar(255) NOT NULL,
  `regdate` date NOT NULL,
  `sex` int(2) NOT NULL,
  `userid` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `mantlecommodity` (
  `id` int(11) NOT NULL,
  `itemID` int(11) NOT NULL,
  `price` int(11) NOT NULL,
  `bargainPrice` int(11) NOT NULL,
  `term` int(11) NOT NULL DEFAULT -1,
  `flag` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC;

CREATE TABLE `petcommodity` (
  `id` int(11) NOT NULL,
  `itemID` int(11) NOT NULL,
  `price` int(11) NOT NULL,
  `bargainPrice` int(11) NOT NULL,
  `term` int(11) NOT NULL DEFAULT -1,
  `flag` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC;

CREATE TABLE `pets` (
  `id` int(11) NOT NULL,
  `cid` int(11) NOT NULL,
  `itemId` int(11) NOT NULL,
  `decorateId` int(11) NOT NULL,
  `name` varchar(20) CHARACTER SET big5 NOT NULL,
  `level` int(11) NOT NULL,
  `hp` int(11) NOT NULL,
  `mp` int(11) NOT NULL,
  `exp` int(11) NOT NULL,
  `type` int(3) NOT NULL,
  `slot` int(3) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC;

CREATE TABLE `producecommodity` (
  `id` int(11) NOT NULL,
  `itemID` int(11) NOT NULL,
  `price` int(11) NOT NULL,
  `bargainPrice` int(11) NOT NULL,
  `term` int(11) NOT NULL DEFAULT -1,
  `flag` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC;

CREATE TABLE `quests` (
  `id` int(11) NOT NULL,
  `cid` int(11) NOT NULL,
  `questId` int(8) NOT NULL,
  `stage` int(3) NOT NULL,
  `completeMonster` int(11) NOT NULL,
  `requireMonster` int(11) NOT NULL,
  `questState` int(3) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC;

CREATE TABLE `skills` (
  `id` int(11) NOT NULL,
  `cid` int(11) NOT NULL,
  `skillId` int(8) NOT NULL,
  `skillLevel` int(3) NOT NULL,
  `type` int(3) NOT NULL,
  `slot` int(3) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC;

CREATE TABLE `storages` (
  `id` int(11) NOT NULL,
  `cid` int(11) NOT NULL,
  `itemID` int(11) NOT NULL DEFAULT 0,
  `quantity` int(11) UNSIGNED NOT NULL DEFAULT 0,
  `spirit` int(11) DEFAULT NULL,
  `level1` int(3) NOT NULL DEFAULT 0,
  `level2` int(3) NOT NULL DEFAULT 0,
  `level3` int(3) NOT NULL DEFAULT 0,
  `level4` int(3) NOT NULL DEFAULT 0,
  `level5` int(3) NOT NULL DEFAULT 0,
  `level6` int(3) NOT NULL DEFAULT 0,
  `level7` int(3) NOT NULL DEFAULT 0,
  `level8` int(3) NOT NULL DEFAULT 0,
  `level9` int(3) NOT NULL DEFAULT 0,
  `level10` int(3) NOT NULL DEFAULT 0,
  `fusion` int(3) NOT NULL DEFAULT 0,
  `isLocked` int(1) NOT NULL DEFAULT 0,
  `icon` int(11) NOT NULL DEFAULT 0,
  `term` int(11) NOT NULL DEFAULT 0,
  `type` int(11) NOT NULL DEFAULT 0,
  `slot` int(11) NOT NULL DEFAULT 0,
  `money` int(10) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC;

CREATE TABLE `talismancommodity` (
  `id` int(11) NOT NULL,
  `itemID` int(11) NOT NULL,
  `price` int(11) NOT NULL,
  `bargainPrice` int(11) NOT NULL,
  `term` int(11) NOT NULL DEFAULT -1,
  `flag` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC;

CREATE TABLE `tb_delete_player_log` (
  `date_delete` date NOT NULL,
  `date_request` int(11) NOT NULL,
  `nickname` varchar(255) NOT NULL,
  `userid` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `tb_guihonman` (
  `addtime` int(4) NOT NULL,
  `ghexp` int(4) NOT NULL,
  `item_exp1` int(4) NOT NULL,
  `item_exp2` int(4) NOT NULL,
  `item_exp3` int(4) NOT NULL,
  `item_exp4` int(4) NOT NULL,
  `item_exp5` int(4) NOT NULL,
  `item_exp6` int(4) NOT NULL,
  `item_exp7` int(4) NOT NULL,
  `item_exp8` int(4) NOT NULL,
  `item_exp9` int(4) NOT NULL,
  `item_exp10` int(4) NOT NULL,
  `item_exp11` int(4) NOT NULL,
  `item_exp12` int(4) NOT NULL,
  `item_id1` int(4) NOT NULL,
  `item_id2` int(4) NOT NULL,
  `item_id3` int(4) NOT NULL,
  `item_id4` int(4) NOT NULL,
  `item_id5` int(4) NOT NULL,
  `item_id6` int(4) NOT NULL,
  `item_id7` int(4) NOT NULL,
  `item_id8` int(4) NOT NULL,
  `item_id9` int(4) NOT NULL,
  `item_id10` int(4) NOT NULL,
  `item_id11` int(4) NOT NULL,
  `item_id12` int(4) NOT NULL,
  `item_unique1` varchar(255) NOT NULL,
  `item_unique2` varchar(255) NOT NULL,
  `item_unique3` varchar(255) NOT NULL,
  `item_unique4` varchar(255) NOT NULL,
  `item_unique5` varchar(255) NOT NULL,
  `item_unique6` varchar(255) NOT NULL,
  `item_unique7` varchar(255) NOT NULL,
  `item_unique8` varchar(255) NOT NULL,
  `item_unique9` varchar(255) NOT NULL,
  `limittime` int(4) NOT NULL,
  `nickname` varchar(255) NOT NULL,
  `useslot` int(4) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `useslot` (
  `id` int(11) NOT NULL,
  `cid` int(11) NOT NULL,
  `type` int(3) NOT NULL,
  `slot` int(3) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC;


ALTER TABLE `accounts`
  ADD PRIMARY KEY (`id`) USING BTREE;

ALTER TABLE `amuletcommodity`
  ADD PRIMARY KEY (`id`) USING BTREE;

ALTER TABLE `boydresscommodity`
  ADD PRIMARY KEY (`id`) USING BTREE;

ALTER TABLE `boyeyescommodity`
  ADD PRIMARY KEY (`id`) USING BTREE;

ALTER TABLE `boyhaircommodity`
  ADD PRIMARY KEY (`id`) USING BTREE;

ALTER TABLE `buycommoditylog`
  ADD PRIMARY KEY (`id`) USING BTREE;

ALTER TABLE `characters`
  ADD PRIMARY KEY (`id`) USING BTREE;

ALTER TABLE `coupon`
  ADD PRIMARY KEY (`id`) USING BTREE;

ALTER TABLE `drop_data`
  ADD PRIMARY KEY (`id`) USING BTREE;

ALTER TABLE `face1commodity`
  ADD PRIMARY KEY (`id`) USING BTREE;

ALTER TABLE `face2commodity`
  ADD PRIMARY KEY (`id`) USING BTREE;

ALTER TABLE `gifts`
  ADD PRIMARY KEY (`id`) USING BTREE;

ALTER TABLE `girldresscommodity`
  ADD PRIMARY KEY (`id`) USING BTREE;

ALTER TABLE `girleyescommodity`
  ADD PRIMARY KEY (`id`) USING BTREE;

ALTER TABLE `girlhaircommodity`
  ADD PRIMARY KEY (`id`) USING BTREE;

ALTER TABLE `hatcommodity`
  ADD PRIMARY KEY (`id`) USING BTREE;

ALTER TABLE `items`
  ADD PRIMARY KEY (`id`) USING BTREE;

ALTER TABLE `mantlecommodity`
  ADD PRIMARY KEY (`id`) USING BTREE;

ALTER TABLE `petcommodity`
  ADD PRIMARY KEY (`id`) USING BTREE;

ALTER TABLE `pets`
  ADD PRIMARY KEY (`id`) USING BTREE;

ALTER TABLE `producecommodity`
  ADD PRIMARY KEY (`id`) USING BTREE;

ALTER TABLE `quests`
  ADD PRIMARY KEY (`id`) USING BTREE;

ALTER TABLE `skills`
  ADD PRIMARY KEY (`id`) USING BTREE;

ALTER TABLE `storages`
  ADD PRIMARY KEY (`id`) USING BTREE;

ALTER TABLE `talismancommodity`
  ADD PRIMARY KEY (`id`) USING BTREE;

ALTER TABLE `useslot`
  ADD PRIMARY KEY (`id`) USING BTREE;


ALTER TABLE `accounts`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

ALTER TABLE `amuletcommodity`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

ALTER TABLE `boydresscommodity`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

ALTER TABLE `boyeyescommodity`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

ALTER TABLE `boyhaircommodity`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

ALTER TABLE `buycommoditylog`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

ALTER TABLE `characters`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

ALTER TABLE `coupon`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

ALTER TABLE `drop_data`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

ALTER TABLE `face1commodity`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

ALTER TABLE `face2commodity`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

ALTER TABLE `gifts`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

ALTER TABLE `girldresscommodity`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

ALTER TABLE `girleyescommodity`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

ALTER TABLE `girlhaircommodity`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

ALTER TABLE `hatcommodity`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

ALTER TABLE `items`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

ALTER TABLE `mantlecommodity`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

ALTER TABLE `petcommodity`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

ALTER TABLE `pets`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

ALTER TABLE `producecommodity`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

ALTER TABLE `quests`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

ALTER TABLE `skills`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

ALTER TABLE `storages`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

ALTER TABLE `talismancommodity`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

ALTER TABLE `useslot`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
