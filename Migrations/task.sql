CREATE TABLE `task` (
    `taskId` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
    `boardId` INT(10) UNSIGNED,
    `userId` INT(10) UNSIGNED,
    `createdDate` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `label` VARCHAR(255),
    `text` VARCHAR(512),
    `color` VARCHAR(20),
    `state` VARCHAR(20),
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