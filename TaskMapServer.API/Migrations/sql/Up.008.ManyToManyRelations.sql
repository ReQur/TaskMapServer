CREATE TABLE IF NOT EXISTS `user_to_board` (
    `userId` INT(10) UNSIGNED NOT NULL,
    `boardId` INT(10) UNSIGNED NOT NULL,
    `access_level` ENUM('read-only', 'edit-access', 'administrating') DEFAULT 'read-only',
    UNIQUE KEY (`userId`, `boardId`),
    FOREIGN KEY (`userId`)
        REFERENCES `user` (`userId`)
        ON DELETE CASCADE
        ON UPDATE NO ACTION,
    FOREIGN KEY (`boardId`)
        REFERENCES `board` (`boardId`)
        ON DELETE CASCADE
        ON UPDATE NO ACTION,
    INDEX USING BTREE (`userId`),
    INDEX USING BTREE (`boardId`)
);
CREATE TRIGGER link_board_to_user_on_creation AFTER INSERT ON board
    FOR EACH ROW
    BEGIN
        DECLARE _boardId INT UNSIGNED DEFAULT NULL;
        DECLARE _userId INT UNSIGNED DEFAULT NULL;
        SELECT MAX(boardId)
        INTO _boardId
        FROM board;
        SELECT userId
        INTO _userId
        FROM board
        WHERE boardId=_boardId;
        INSERT INTO user_to_board(userId, boardId, access_level)
                   VALUES(_userId, _boardId, 'administrating');
    END;

INSERT INTO user_to_board
select *, 'administrating' as access_level
from (SELECT userId, boardId
      from board) as uIbI;
