ALTER TABLE `user`
    RENAME COLUMN `md5PasswordHash` TO `password`;

ALTER TABLE `user`
    ADD COLUMN `avatar` VARCHAR(255);