CREATE TABLE IF NOT EXISTS `user` (
    `userId` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
    `email` VARCHAR(255) NOT NULL,
    `firstName` VARCHAR(255) NOT NULL,
    `lastName` VARCHAR(255) NOT NULL,
    `md5PasswordHash` VARCHAR(255) NOT NULL,
    `lastBoardId` INT(10) UNSIGNED,
    PRIMARY KEY (`userId`),
    UNIQUE KEY (`email`)
);
CREATE TABLE `board` (
  `boardId` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `userId` INT(10) UNSIGNED,
  `createdDate` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `boardName` VARCHAR(255),
  `boardDescription` VARCHAR(512),
  `state` VARCHAR(20),
  PRIMARY KEY (`boardId`),
  CONSTRAINT `fk_board_user_userId`
    FOREIGN KEY (`userId`)
    REFERENCES `user` (`userId`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
  );
CREATE TABLE `task` (
    `taskId` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
    `boardId` INT(10) UNSIGNED,
    `userId` INT(10) UNSIGNED,
    `createdDate` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `taskLabel` VARCHAR(255),
    `taskText` VARCHAR(512),
    `taskColor` VARCHAR(20),
    `state` int(2) UNSIGNED,
    `coordinates` VARCHAR(255),
    PRIMARY KEY (`taskId`),
    CONSTRAINT `fk_task_user_userId`
        FOREIGN KEY (`userId`)
        REFERENCES `user` (`userId`)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION,
    CONSTRAINT `fk_task_board_boardId`
        FOREIGN KEY (`boardId`)
        REFERENCES `board` (`boardId`)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION
  );