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