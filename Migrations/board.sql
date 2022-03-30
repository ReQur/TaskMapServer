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


INSERT INTO board(userId, boardName, boardDescription, state) VALUES(1, 'testBoard', 'That is test board', 'what');