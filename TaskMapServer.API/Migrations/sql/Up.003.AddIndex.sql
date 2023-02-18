CREATE INDEX boardId_idx
	USING BTREE
	ON task (boardId);
CREATE INDEX userId_idx
	USING BTREE
	ON board (userId);