ALTER TABLE board
DROP CONSTRAINT fk_board_user_userId,
ADD CONSTRAINT fk_board_user_userId_exists
    FOREIGN KEY (`userId`)
    REFERENCES `user` (`userId`)
    ON DELETE CASCADE
    ON UPDATE NO ACTION;

ALTER TABLE task
DROP CONSTRAINT fk_task_user_userId,
ADD CONSTRAINT fk_task_user_userId_exists
    FOREIGN KEY (`userId`)
    REFERENCES `user` (`userId`)
    ON DELETE CASCADE
    ON UPDATE NO ACTION;

ALTER TABLE task
DROP CONSTRAINT fk_task_board_boardId,
ADD CONSTRAINT fk_board_with_boardId_exists
    FOREIGN KEY (`boardId`)
    REFERENCES `board` (`boardId`)
    ON DELETE CASCADE
    ON UPDATE NO ACTION;