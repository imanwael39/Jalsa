-- ============================================================
-- Jalsa Seed Data (additive) - generated 2026-07-06T12:20:48.724Z
-- Adds on top of existing Galsa_DB data. Reuses existing Roles
-- (Admin/Patient/Therapist) and AssessmentTemplates (gad-7, dass-21).
-- All seeded user accounts share password: Password123!
-- ============================================================
SET NOCOUNT ON;
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
SET XACT_ABORT ON;
BEGIN TRANSACTION;


-- ===== Users =====
INSERT INTO [Users] ([Id], [Email], [PasswordHash], [IsActive], [LastLoginAt], [FailedLoginAttempts], [LockoutEnd], [CreatedAt], [UpdatedAt])
VALUES
('2D19FD6D-560A-451B-BC1F-4E446A0749B1', N'admin@jalsaclinic.com', N'$2a$11$FA4nXV/knGgZW1/WCVaBxe9/O8dhTg9T1.C.jOGBv/nR6dD5kp6eu', 1, '2026-07-05 12:00:00', 0, NULL, '2025-06-01 12:00:00', '2025-06-01 12:00:00'),
('8707DD3C-5834-4909-81E1-69CFA4AB12B5', N'ahmed.elsayed@jalsaclinic.com', N'$2a$11$FA4nXV/knGgZW1/WCVaBxe9/O8dhTg9T1.C.jOGBv/nR6dD5kp6eu', 1, '2026-07-01 12:00:00', 0, NULL, '2025-08-19 12:00:00', '2025-08-19 12:00:00'),
('D13A0975-0432-4FAA-86BB-8CE87DC80AD3', N'mona.abdelrahman@jalsaclinic.com', N'$2a$11$FA4nXV/knGgZW1/WCVaBxe9/O8dhTg9T1.C.jOGBv/nR6dD5kp6eu', 1, '2026-07-01 12:00:00', 0, NULL, '2025-09-11 12:00:00', '2025-09-11 12:00:00'),
('645DD596-ACBA-4A99-B148-9E840EBDBC42', N'karim.hussein@jalsaclinic.com', N'$2a$11$FA4nXV/knGgZW1/WCVaBxe9/O8dhTg9T1.C.jOGBv/nR6dD5kp6eu', 1, '2026-07-02 12:00:00', 0, NULL, '2025-06-14 12:00:00', '2025-06-14 12:00:00'),
('890607A3-3F87-400F-95A1-42171F2F95DD', N'laila.farouk@jalsaclinic.com', N'$2a$11$FA4nXV/knGgZW1/WCVaBxe9/O8dhTg9T1.C.jOGBv/nR6dD5kp6eu', 1, '2026-07-06 12:00:00', 0, NULL, '2025-08-02 12:00:00', '2025-08-02 12:00:00'),
('02A6C5EC-E76C-443D-AA05-C6C4FBEB839B', N'youssef.mansour@jalsaclinic.com', N'$2a$11$FA4nXV/knGgZW1/WCVaBxe9/O8dhTg9T1.C.jOGBv/nR6dD5kp6eu', 1, '2026-07-05 12:00:00', 0, NULL, '2025-07-07 12:00:00', '2025-07-07 12:00:00'),
('0759431C-65BC-49A2-94FE-E906EBF11418', N'heba.ibrahim@jalsaclinic.com', N'$2a$11$FA4nXV/knGgZW1/WCVaBxe9/O8dhTg9T1.C.jOGBv/nR6dD5kp6eu', 1, '2026-07-03 12:00:00', 0, NULL, '2025-10-27 12:00:00', '2025-10-27 12:00:00'),
('58D0A013-7F67-4666-AC48-44508CC29938', N'tarek.adly@jalsaclinic.com', N'$2a$11$FA4nXV/knGgZW1/WCVaBxe9/O8dhTg9T1.C.jOGBv/nR6dD5kp6eu', 1, '2026-07-04 12:00:00', 0, NULL, '2025-09-26 12:00:00', '2025-09-26 12:00:00'),
('8D78ED99-7BD8-4ED9-B888-243669B55907', N'nadia.sherif@jalsaclinic.com', N'$2a$11$FA4nXV/knGgZW1/WCVaBxe9/O8dhTg9T1.C.jOGBv/nR6dD5kp6eu', 1, '2026-07-05 12:00:00', 0, NULL, '2025-08-03 12:00:00', '2025-08-03 12:00:00'),
('B06C2022-7181-4B83-9000-D2D7BCAA14F8', N'lamia.71@hotmail.com', N'$2a$11$FA4nXV/knGgZW1/WCVaBxe9/O8dhTg9T1.C.jOGBv/nR6dD5kp6eu', 1, '2026-07-03 12:00:00', 0, NULL, '2026-07-03 12:00:00', '2026-07-03 12:00:00'),
('F9C10CB6-CD45-47B2-9BB6-91CC985F8DC5', N'fatma.15@hotmail.com', N'$2a$11$FA4nXV/knGgZW1/WCVaBxe9/O8dhTg9T1.C.jOGBv/nR6dD5kp6eu', 1, '2026-06-17 12:00:00', 0, NULL, '2026-05-20 12:00:00', '2026-05-20 12:00:00'),
('262F5E5C-0885-4572-BD7A-247EFD15D376', N'amal.98@gmail.com', N'$2a$11$FA4nXV/knGgZW1/WCVaBxe9/O8dhTg9T1.C.jOGBv/nR6dD5kp6eu', 1, '2026-06-23 12:00:00', 0, NULL, '2025-06-27 12:00:00', '2025-06-27 12:00:00'),
('8E6E5EAC-2176-4F0B-9C3D-BADAEF969807', N'mark.thompson44@yahoo.com', N'$2a$11$FA4nXV/knGgZW1/WCVaBxe9/O8dhTg9T1.C.jOGBv/nR6dD5kp6eu', 1, '2026-06-26 12:00:00', 0, NULL, '2025-11-26 12:00:00', '2025-11-26 12:00:00'),
('E0FFDBC2-F09B-4996-88E4-9F4B23D6F0FF', N'khaled.42@gmail.com', N'$2a$11$FA4nXV/knGgZW1/WCVaBxe9/O8dhTg9T1.C.jOGBv/nR6dD5kp6eu', 1, NULL, 0, NULL, '2025-09-23 12:00:00', '2025-09-23 12:00:00'),
('72F60FBA-CE9B-41DC-B960-1EAC6D43143D', N'reham.99@hotmail.com', N'$2a$11$FA4nXV/knGgZW1/WCVaBxe9/O8dhTg9T1.C.jOGBv/nR6dD5kp6eu', 1, NULL, 0, NULL, '2026-03-07 12:00:00', '2026-03-07 12:00:00'),
('8F464A15-DAAB-4952-BF75-064FCFD765E7', N'reham.92@yahoo.com', N'$2a$11$FA4nXV/knGgZW1/WCVaBxe9/O8dhTg9T1.C.jOGBv/nR6dD5kp6eu', 1, NULL, 0, NULL, '2026-01-20 12:00:00', '2026-01-20 12:00:00'),
('A4D9297C-F020-4C75-8F70-207DD99016FE', N'daniel.moore82@gmail.com', N'$2a$11$FA4nXV/knGgZW1/WCVaBxe9/O8dhTg9T1.C.jOGBv/nR6dD5kp6eu', 1, '2026-06-27 12:00:00', 0, NULL, '2026-02-02 12:00:00', '2026-02-02 12:00:00'),
('DB091DDD-6138-42BD-B1BD-F8D5DCF6DF96', N'dina.46@yahoo.com', N'$2a$11$FA4nXV/knGgZW1/WCVaBxe9/O8dhTg9T1.C.jOGBv/nR6dD5kp6eu', 1, NULL, 0, NULL, '2026-06-16 12:00:00', '2026-06-16 12:00:00'),
('65558633-FEE9-48B7-8AF8-40BFD220B851', N'sarah.davis72@gmail.com', N'$2a$11$FA4nXV/knGgZW1/WCVaBxe9/O8dhTg9T1.C.jOGBv/nR6dD5kp6eu', 1, '2026-06-25 12:00:00', 0, NULL, '2026-02-21 12:00:00', '2026-02-21 12:00:00'),
('82C584D7-9870-4182-8752-2AB1097712D3', N'malak.85@outlook.com', N'$2a$11$FA4nXV/knGgZW1/WCVaBxe9/O8dhTg9T1.C.jOGBv/nR6dD5kp6eu', 1, '2026-06-17 12:00:00', 0, NULL, '2026-03-28 12:00:00', '2026-03-28 12:00:00'),
('AECAB352-8D01-4258-9451-A98DA466761F', N'basem.51@outlook.com', N'$2a$11$FA4nXV/knGgZW1/WCVaBxe9/O8dhTg9T1.C.jOGBv/nR6dD5kp6eu', 1, NULL, 0, NULL, '2026-01-16 12:00:00', '2026-01-16 12:00:00'),
('ED2A4F7A-6392-466E-9979-51CD2AC1FE2E', N'salma.48@hotmail.com', N'$2a$11$FA4nXV/knGgZW1/WCVaBxe9/O8dhTg9T1.C.jOGBv/nR6dD5kp6eu', 1, '2026-07-01 12:00:00', 0, NULL, '2026-01-10 12:00:00', '2026-01-10 12:00:00'),
('A45399A0-E659-442B-8C4A-F0378C9E2347', N'mark.thompson27@gmail.com', N'$2a$11$FA4nXV/knGgZW1/WCVaBxe9/O8dhTg9T1.C.jOGBv/nR6dD5kp6eu', 1, NULL, 0, NULL, '2026-04-08 12:00:00', '2026-04-08 12:00:00'),
('3E18E043-8106-4F93-A0C5-8BFECBF20132', N'yasser.61@yahoo.com', N'$2a$11$FA4nXV/knGgZW1/WCVaBxe9/O8dhTg9T1.C.jOGBv/nR6dD5kp6eu', 1, NULL, 0, NULL, '2026-04-28 12:00:00', '2026-04-28 12:00:00'),
('A7CCD1A6-1AEF-426F-A1D4-DAAA45EEFCD8', N'tarek.82@gmail.com', N'$2a$11$FA4nXV/knGgZW1/WCVaBxe9/O8dhTg9T1.C.jOGBv/nR6dD5kp6eu', 1, '2026-06-25 12:00:00', 0, NULL, '2025-09-09 12:00:00', '2025-09-09 12:00:00'),
('250B7FE2-5C94-432C-89B9-80AF87185FC0', N'jessica.taylor38@gmail.com', N'$2a$11$FA4nXV/knGgZW1/WCVaBxe9/O8dhTg9T1.C.jOGBv/nR6dD5kp6eu', 1, NULL, 0, NULL, '2025-09-30 12:00:00', '2025-09-30 12:00:00'),
('40AF507D-2E94-40AA-9268-B0BBA290493B', N'john.carter82@gmail.com', N'$2a$11$FA4nXV/knGgZW1/WCVaBxe9/O8dhTg9T1.C.jOGBv/nR6dD5kp6eu', 1, '2026-07-01 12:00:00', 0, NULL, '2025-11-13 12:00:00', '2025-11-13 12:00:00'),
('E64A2B73-C807-421A-BBDE-A9FCA7D29587', N'omar.17@yahoo.com', N'$2a$11$FA4nXV/knGgZW1/WCVaBxe9/O8dhTg9T1.C.jOGBv/nR6dD5kp6eu', 1, NULL, 0, NULL, '2026-04-01 12:00:00', '2026-04-01 12:00:00'),
('B672F83A-8C74-4251-BB3F-AF46E9017E52', N'ayman.50@yahoo.com', N'$2a$11$FA4nXV/knGgZW1/WCVaBxe9/O8dhTg9T1.C.jOGBv/nR6dD5kp6eu', 1, '2026-06-27 12:00:00', 0, NULL, '2025-12-07 12:00:00', '2025-12-07 12:00:00'),
('C16CE25F-85AE-4B1D-BDF1-05FCAAE4FE41', N'malak.32@yahoo.com', N'$2a$11$FA4nXV/knGgZW1/WCVaBxe9/O8dhTg9T1.C.jOGBv/nR6dD5kp6eu', 1, NULL, 0, NULL, '2025-07-17 12:00:00', '2025-07-17 12:00:00'),
('C7A4E929-6F01-4043-B3F3-D1A01505C894', N'mona.79@gmail.com', N'$2a$11$FA4nXV/knGgZW1/WCVaBxe9/O8dhTg9T1.C.jOGBv/nR6dD5kp6eu', 1, '2026-06-20 12:00:00', 0, NULL, '2026-02-19 12:00:00', '2026-02-19 12:00:00'),
('6F7A2849-8BFC-45CB-83E5-F305BF4B7ADF', N'robert.anderson37@gmail.com', N'$2a$11$FA4nXV/knGgZW1/WCVaBxe9/O8dhTg9T1.C.jOGBv/nR6dD5kp6eu', 1, '2026-07-03 12:00:00', 0, NULL, '2026-06-28 12:00:00', '2026-06-28 12:00:00'),
('43FC9F53-CC9C-4DCF-8670-68CA91084378', N'mariam.48@outlook.com', N'$2a$11$FA4nXV/knGgZW1/WCVaBxe9/O8dhTg9T1.C.jOGBv/nR6dD5kp6eu', 1, NULL, 0, NULL, '2026-01-26 12:00:00', '2026-01-26 12:00:00'),
('4F2CF45C-16B5-4BB6-85BE-3F63C6D1091C', N'ayman.25@gmail.com', N'$2a$11$FA4nXV/knGgZW1/WCVaBxe9/O8dhTg9T1.C.jOGBv/nR6dD5kp6eu', 1, '2026-06-20 12:00:00', 0, NULL, '2026-06-20 12:00:00', '2026-06-20 12:00:00'),
('C652B582-2B56-43AC-ABF6-A78D796BBD93', N'omar.55@yahoo.com', N'$2a$11$FA4nXV/knGgZW1/WCVaBxe9/O8dhTg9T1.C.jOGBv/nR6dD5kp6eu', 1, NULL, 0, NULL, '2026-03-07 12:00:00', '2026-03-07 12:00:00'),
('9F374DC9-631C-4155-B308-FA58C6FDB75B', N'fatma.78@yahoo.com', N'$2a$11$FA4nXV/knGgZW1/WCVaBxe9/O8dhTg9T1.C.jOGBv/nR6dD5kp6eu', 1, NULL, 0, NULL, '2026-02-08 12:00:00', '2026-02-08 12:00:00'),
('1C257B5A-D9E6-410F-9BAC-1744E71E1FE7', N'robert.anderson19@outlook.com', N'$2a$11$FA4nXV/knGgZW1/WCVaBxe9/O8dhTg9T1.C.jOGBv/nR6dD5kp6eu', 1, '2026-06-23 12:00:00', 0, NULL, '2026-02-20 12:00:00', '2026-02-20 12:00:00'),
('F7F7FD61-459C-402D-A380-067F51857677', N'james.smith87@yahoo.com', N'$2a$11$FA4nXV/knGgZW1/WCVaBxe9/O8dhTg9T1.C.jOGBv/nR6dD5kp6eu', 1, NULL, 0, NULL, '2026-06-02 12:00:00', '2026-06-02 12:00:00'),
('9F654540-0CB3-4374-AACF-0E72E6957C1E', N'maged.29@yahoo.com', N'$2a$11$FA4nXV/knGgZW1/WCVaBxe9/O8dhTg9T1.C.jOGBv/nR6dD5kp6eu', 1, '2026-07-01 12:00:00', 0, NULL, '2026-01-21 12:00:00', '2026-01-21 12:00:00'),
('1B4874E3-F13E-4B41-96F2-A4DF4E747070', N'yasser.99@yahoo.com', N'$2a$11$FA4nXV/knGgZW1/WCVaBxe9/O8dhTg9T1.C.jOGBv/nR6dD5kp6eu', 1, '2026-07-03 12:00:00', 0, NULL, '2026-06-02 12:00:00', '2026-06-02 12:00:00'),
('22518D39-E6EC-4742-82D4-A9A14410326C', N'waleed.31@hotmail.com', N'$2a$11$FA4nXV/knGgZW1/WCVaBxe9/O8dhTg9T1.C.jOGBv/nR6dD5kp6eu', 1, NULL, 0, NULL, '2026-03-12 12:00:00', '2026-03-12 12:00:00'),
('38E2BAFB-3323-4DCB-83E0-192A6E91F5DD', N'ramy.52@hotmail.com', N'$2a$11$FA4nXV/knGgZW1/WCVaBxe9/O8dhTg9T1.C.jOGBv/nR6dD5kp6eu', 1, NULL, 0, NULL, '2026-03-07 12:00:00', '2026-03-07 12:00:00'),
('8C82ABCC-10FE-487B-9F8F-1E502FF589FD', N'amal.32@outlook.com', N'$2a$11$FA4nXV/knGgZW1/WCVaBxe9/O8dhTg9T1.C.jOGBv/nR6dD5kp6eu', 1, '2026-07-05 12:00:00', 0, NULL, '2025-11-18 12:00:00', '2025-11-18 12:00:00'),
('9CED10E6-0BFB-4D43-9656-019687D99BC4', N'james.smith72@outlook.com', N'$2a$11$FA4nXV/knGgZW1/WCVaBxe9/O8dhTg9T1.C.jOGBv/nR6dD5kp6eu', 1, '2026-07-01 12:00:00', 0, NULL, '2025-09-03 12:00:00', '2025-09-03 12:00:00');

-- ===== UserRoles =====
INSERT INTO [UserRoles] ([UserId], [RoleId], [CreatedAt])
VALUES
('2D19FD6D-560A-451B-BC1F-4E446A0749B1', '83401DA6-DD30-44E7-9612-1D424B1438CF', '2025-06-01 12:00:00'),
('8707DD3C-5834-4909-81E1-69CFA4AB12B5', '573C688C-CAEE-47B2-A35F-ADFD4F7C1F9C', '2025-08-19 12:00:00'),
('D13A0975-0432-4FAA-86BB-8CE87DC80AD3', '573C688C-CAEE-47B2-A35F-ADFD4F7C1F9C', '2025-09-11 12:00:00'),
('645DD596-ACBA-4A99-B148-9E840EBDBC42', '573C688C-CAEE-47B2-A35F-ADFD4F7C1F9C', '2025-06-14 12:00:00'),
('890607A3-3F87-400F-95A1-42171F2F95DD', '573C688C-CAEE-47B2-A35F-ADFD4F7C1F9C', '2025-08-02 12:00:00'),
('02A6C5EC-E76C-443D-AA05-C6C4FBEB839B', '573C688C-CAEE-47B2-A35F-ADFD4F7C1F9C', '2025-07-07 12:00:00'),
('0759431C-65BC-49A2-94FE-E906EBF11418', '573C688C-CAEE-47B2-A35F-ADFD4F7C1F9C', '2025-10-27 12:00:00'),
('58D0A013-7F67-4666-AC48-44508CC29938', '573C688C-CAEE-47B2-A35F-ADFD4F7C1F9C', '2025-09-26 12:00:00'),
('8D78ED99-7BD8-4ED9-B888-243669B55907', '573C688C-CAEE-47B2-A35F-ADFD4F7C1F9C', '2025-08-03 12:00:00'),
('B06C2022-7181-4B83-9000-D2D7BCAA14F8', '705934BD-F6E9-4FD4-B141-57FDE9C78AD6', '2026-07-03 12:00:00'),
('F9C10CB6-CD45-47B2-9BB6-91CC985F8DC5', '705934BD-F6E9-4FD4-B141-57FDE9C78AD6', '2026-05-20 12:00:00'),
('262F5E5C-0885-4572-BD7A-247EFD15D376', '705934BD-F6E9-4FD4-B141-57FDE9C78AD6', '2025-06-27 12:00:00'),
('8E6E5EAC-2176-4F0B-9C3D-BADAEF969807', '705934BD-F6E9-4FD4-B141-57FDE9C78AD6', '2025-11-26 12:00:00'),
('E0FFDBC2-F09B-4996-88E4-9F4B23D6F0FF', '705934BD-F6E9-4FD4-B141-57FDE9C78AD6', '2025-09-23 12:00:00'),
('72F60FBA-CE9B-41DC-B960-1EAC6D43143D', '705934BD-F6E9-4FD4-B141-57FDE9C78AD6', '2026-03-07 12:00:00'),
('8F464A15-DAAB-4952-BF75-064FCFD765E7', '705934BD-F6E9-4FD4-B141-57FDE9C78AD6', '2026-01-20 12:00:00'),
('A4D9297C-F020-4C75-8F70-207DD99016FE', '705934BD-F6E9-4FD4-B141-57FDE9C78AD6', '2026-02-02 12:00:00'),
('DB091DDD-6138-42BD-B1BD-F8D5DCF6DF96', '705934BD-F6E9-4FD4-B141-57FDE9C78AD6', '2026-06-16 12:00:00'),
('65558633-FEE9-48B7-8AF8-40BFD220B851', '705934BD-F6E9-4FD4-B141-57FDE9C78AD6', '2026-02-21 12:00:00'),
('82C584D7-9870-4182-8752-2AB1097712D3', '705934BD-F6E9-4FD4-B141-57FDE9C78AD6', '2026-03-28 12:00:00'),
('AECAB352-8D01-4258-9451-A98DA466761F', '705934BD-F6E9-4FD4-B141-57FDE9C78AD6', '2026-01-16 12:00:00'),
('ED2A4F7A-6392-466E-9979-51CD2AC1FE2E', '705934BD-F6E9-4FD4-B141-57FDE9C78AD6', '2026-01-10 12:00:00'),
('A45399A0-E659-442B-8C4A-F0378C9E2347', '705934BD-F6E9-4FD4-B141-57FDE9C78AD6', '2026-04-08 12:00:00'),
('3E18E043-8106-4F93-A0C5-8BFECBF20132', '705934BD-F6E9-4FD4-B141-57FDE9C78AD6', '2026-04-28 12:00:00'),
('A7CCD1A6-1AEF-426F-A1D4-DAAA45EEFCD8', '705934BD-F6E9-4FD4-B141-57FDE9C78AD6', '2025-09-09 12:00:00'),
('250B7FE2-5C94-432C-89B9-80AF87185FC0', '705934BD-F6E9-4FD4-B141-57FDE9C78AD6', '2025-09-30 12:00:00'),
('40AF507D-2E94-40AA-9268-B0BBA290493B', '705934BD-F6E9-4FD4-B141-57FDE9C78AD6', '2025-11-13 12:00:00'),
('E64A2B73-C807-421A-BBDE-A9FCA7D29587', '705934BD-F6E9-4FD4-B141-57FDE9C78AD6', '2026-04-01 12:00:00'),
('B672F83A-8C74-4251-BB3F-AF46E9017E52', '705934BD-F6E9-4FD4-B141-57FDE9C78AD6', '2025-12-07 12:00:00'),
('C16CE25F-85AE-4B1D-BDF1-05FCAAE4FE41', '705934BD-F6E9-4FD4-B141-57FDE9C78AD6', '2025-07-17 12:00:00'),
('C7A4E929-6F01-4043-B3F3-D1A01505C894', '705934BD-F6E9-4FD4-B141-57FDE9C78AD6', '2026-02-19 12:00:00'),
('6F7A2849-8BFC-45CB-83E5-F305BF4B7ADF', '705934BD-F6E9-4FD4-B141-57FDE9C78AD6', '2026-06-28 12:00:00'),
('43FC9F53-CC9C-4DCF-8670-68CA91084378', '705934BD-F6E9-4FD4-B141-57FDE9C78AD6', '2026-01-26 12:00:00'),
('4F2CF45C-16B5-4BB6-85BE-3F63C6D1091C', '705934BD-F6E9-4FD4-B141-57FDE9C78AD6', '2026-06-20 12:00:00'),
('C652B582-2B56-43AC-ABF6-A78D796BBD93', '705934BD-F6E9-4FD4-B141-57FDE9C78AD6', '2026-03-07 12:00:00'),
('9F374DC9-631C-4155-B308-FA58C6FDB75B', '705934BD-F6E9-4FD4-B141-57FDE9C78AD6', '2026-02-08 12:00:00'),
('1C257B5A-D9E6-410F-9BAC-1744E71E1FE7', '705934BD-F6E9-4FD4-B141-57FDE9C78AD6', '2026-02-20 12:00:00'),
('F7F7FD61-459C-402D-A380-067F51857677', '705934BD-F6E9-4FD4-B141-57FDE9C78AD6', '2026-06-02 12:00:00'),
('9F654540-0CB3-4374-AACF-0E72E6957C1E', '705934BD-F6E9-4FD4-B141-57FDE9C78AD6', '2026-01-21 12:00:00'),
('1B4874E3-F13E-4B41-96F2-A4DF4E747070', '705934BD-F6E9-4FD4-B141-57FDE9C78AD6', '2026-06-02 12:00:00'),
('22518D39-E6EC-4742-82D4-A9A14410326C', '705934BD-F6E9-4FD4-B141-57FDE9C78AD6', '2026-03-12 12:00:00'),
('38E2BAFB-3323-4DCB-83E0-192A6E91F5DD', '705934BD-F6E9-4FD4-B141-57FDE9C78AD6', '2026-03-07 12:00:00'),
('8C82ABCC-10FE-487B-9F8F-1E502FF589FD', '705934BD-F6E9-4FD4-B141-57FDE9C78AD6', '2025-11-18 12:00:00'),
('9CED10E6-0BFB-4D43-9656-019687D99BC4', '705934BD-F6E9-4FD4-B141-57FDE9C78AD6', '2025-09-03 12:00:00');

-- ===== Clinics =====
INSERT INTO [Clinics] ([Id], [Name], [Timezone], [CreatedAt])
VALUES
('7A3B78EC-390F-435C-BBD3-B8E4DC10DDB3', N'عيادة جلسة - المعادي', N'Africa/Cairo', '2025-05-12 12:00:00'),
('D612A00E-4DDD-40C2-AF86-E16C43DDA678', N'عيادة جلسة - مدينة نصر', N'Africa/Cairo', '2025-05-12 12:00:00');

-- ===== AssessmentTemplates (new) =====
INSERT INTO [AssessmentTemplates] ([Id], [Name], [Version], [Description], [IsActive], [CreatedAt])
VALUES
('36B3C4A8-51E1-4BEC-B4A9-746B83073EE9', N'phq-9', 1, N'مقياس تقييم أعراض الاكتئاب المكون من 9 بنود (Patient Health Questionnaire-9)', 1, '2025-05-12 12:00:00');

-- ===== Users (Admin + Therapists + Patients) =====

-- ===== Therapists =====
INSERT INTO [Therapists] ([Id], [UserId], [FullName], [LicenseNumber], [Specialization], [Phone], [ProfileImageUrl], [Bio], [CreatedAt])
VALUES
('6251ABB6-B294-48CA-9B75-4E959CFA04A7', '8707DD3C-5834-4909-81E1-69CFA4AB12B5', N'د. أحمد السيد', N'PSY-EGY-2026-101', N'Anxiety Disorders', N'+201570497521', N'https://i.pravatar.cc/300?img=11', N'أخصائي نفسي إكلينيكي بخبرة تزيد عن 10 سنوات في علاج اضطرابات القلق ونوبات الهلع، حاصل على زمالة المعهد القومي للصحة النفسية.', '2025-08-19 12:00:00'),
('0D30427F-E161-441E-AA05-F6749620088C', 'D13A0975-0432-4FAA-86BB-8CE87DC80AD3', N'د. منى عبد الرحمن', N'PSY-EGY-2026-102', N'Depression', N'+201227019960', N'https://i.pravatar.cc/300?img=12', N'أخصائية نفسية متخصصة في علاج الاكتئاب واضطرابات المزاج باستخدام العلاج المعرفي السلوكي.', '2025-09-11 12:00:00'),
('25A9DCC1-6815-47F4-86AD-D25FF47B7ADE', '645DD596-ACBA-4A99-B148-9E840EBDBC42', N'Dr. Karim Hussein', N'PSY-EGY-2026-103', N'Family Therapy', N'+201277594510', N'https://i.pravatar.cc/300?img=13', N'Licensed family therapist with over 12 years of experience helping families navigate conflict, communication, and transitions.', '2025-06-14 12:00:00'),
('DDDA597C-2D47-4C64-914D-6DF694AA3606', '890607A3-3F87-400F-95A1-42171F2F95DD', N'د. ليلى فاروق', N'PSY-EGY-2026-104', N'Trauma Therapy', N'+201217098686', N'https://i.pravatar.cc/300?img=14', N'معالجة نفسية متخصصة في علاج الصدمات النفسية واضطراب ما بعد الصدمة (PTSD) باستخدام تقنيات EMDR.', '2025-08-02 12:00:00'),
('899B50C5-FFCF-4E6E-964D-3A59908E422E', '02A6C5EC-E76C-443D-AA05-C6C4FBEB839B', N'Dr. Youssef Mansour', N'PSY-EGY-2026-105', N'Cognitive Behavioral Therapy (CBT)', N'+201232569292', N'https://i.pravatar.cc/300?img=15', N'Clinical psychologist specializing in Cognitive Behavioral Therapy (CBT) for anxiety, depression, and stress-related disorders.', '2025-07-07 12:00:00'),
('E73DBDDC-8CD8-42F3-A385-E5D24710E59F', '0759431C-65BC-49A2-94FE-E906EBF11418', N'د. هبة إبراهيم', N'PSY-EGY-2026-106', N'Addiction Recovery', N'+201046070258', N'https://i.pravatar.cc/300?img=16', N'أخصائية نفسية متخصصة في علاج الإدمان والتعافي، تدعم المرضى في رحلة التعافي طويل المدى ومنع الانتكاس.', '2025-10-27 12:00:00'),
('770DD106-350E-40A8-B1BC-4CD0BED58C40', '58D0A013-7F67-4666-AC48-44508CC29938', N'Dr. Tarek Adly', N'PSY-EGY-2026-107', N'Child Psychology', N'+201565936782', N'https://i.pravatar.cc/300?img=17', N'Child psychologist with a decade of experience supporting children and adolescents through developmental and behavioral challenges.', '2025-09-26 12:00:00'),
('9F442928-B53E-4FDC-B241-988969B52203', '8D78ED99-7BD8-4ED9-B888-243669B55907', N'د. نادية شريف', N'PSY-EGY-2026-108', N'Stress Management', N'+201014999586', N'https://i.pravatar.cc/300?img=18', N'أخصائية نفسية متخصصة في إدارة الضغوط والإرهاق الوظيفي (Burnout)، تستخدم أساليب اليقظة الذهنية والاسترخاء.', '2025-08-03 12:00:00');

-- ===== TherapistClinics =====
INSERT INTO [TherapistClinics] ([TherapistId], [ClinicId], [IsPrimary])
VALUES
('6251ABB6-B294-48CA-9B75-4E959CFA04A7', '7A3B78EC-390F-435C-BBD3-B8E4DC10DDB3', 1),
('0D30427F-E161-441E-AA05-F6749620088C', 'D612A00E-4DDD-40C2-AF86-E16C43DDA678', 1),
('25A9DCC1-6815-47F4-86AD-D25FF47B7ADE', '7A3B78EC-390F-435C-BBD3-B8E4DC10DDB3', 1),
('DDDA597C-2D47-4C64-914D-6DF694AA3606', 'D612A00E-4DDD-40C2-AF86-E16C43DDA678', 1),
('899B50C5-FFCF-4E6E-964D-3A59908E422E', '7A3B78EC-390F-435C-BBD3-B8E4DC10DDB3', 1),
('E73DBDDC-8CD8-42F3-A385-E5D24710E59F', 'D612A00E-4DDD-40C2-AF86-E16C43DDA678', 1),
('770DD106-350E-40A8-B1BC-4CD0BED58C40', '7A3B78EC-390F-435C-BBD3-B8E4DC10DDB3', 1),
('9F442928-B53E-4FDC-B241-988969B52203', 'D612A00E-4DDD-40C2-AF86-E16C43DDA678', 1);

-- ===== Patients =====
INSERT INTO [Patients] ([Id], [TherapistId], [ClinicId], [UserId], [FullName], [DateOfBirth], [Gender], [Phone], [Email], [Address], [ReferralSource], [ChiefComplaint], [EmergencyContact], [MedicalHistory], [ProfileImageUrl], [Status], [CreatedAt], [UpdatedAt])
VALUES
('692BFE83-C7BF-4FAA-9245-0EEDE315EBDB', '6251ABB6-B294-48CA-9B75-4E959CFA04A7', '7A3B78EC-390F-435C-BBD3-B8E4DC10DDB3', 'B06C2022-7181-4B83-9000-D2D7BCAA14F8', N'لمياء سعيد', '1964-04-21', N'Female', N'+201160995682', N'lamia.71@hotmail.com', N'الإسكندرية', N'وسائل التواصل الاجتماعي', N'ضغط العمل (Work stress)', N'الأخت - سارة عبدالله - +201190980339', N'تاريخ عائلي للإصابة بالاكتئاب.', N'https://i.pravatar.cc/300?img=1', N'Active', '2026-07-03 12:00:00', '2026-07-03 12:00:00'),
('5912AA70-D127-4B2A-95B6-62222A464244', '0D30427F-E161-441E-AA05-F6749620088C', 'D612A00E-4DDD-40C2-AF86-E16C43DDA678', 'F9C10CB6-CD45-47B2-9BB6-91CC985F8DC5', N'فاطمة زكي', '1990-10-21', N'Female', N'+201092039345', N'fatma.15@hotmail.com', N'الرياض، السعودية', N'بحث عبر الإنترنت', N'اضطراب النوم (Sleep disorder)', N'الزوج - سارة زكي - +201274039828', N'تاريخ من اضطرابات النوم منذ المراهقة.', N'https://i.pravatar.cc/300?img=2', N'Active', '2026-05-20 12:00:00', '2026-05-20 12:00:00'),
('D091A7F4-A73A-4CCE-920E-0AB7D4C36536', '25A9DCC1-6815-47F4-86AD-D25FF47B7ADE', '7A3B78EC-390F-435C-BBD3-B8E4DC10DDB3', '262F5E5C-0885-4572-BD7A-247EFD15D376', N'أمل نصار', '1968-09-06', N'Female', N'+201575811644', N'amal.98@gmail.com', N'مصر الجديدة، القاهرة', N'إحالة من جهة العمل', N'قلق اجتماعي (Social anxiety)', N'الأخت - منى صبحي - +201025779044', N'لا توجد أمراض جسدية مزمنة.', N'https://i.pravatar.cc/300?img=3', N'Active', '2025-06-27 12:00:00', '2025-06-27 12:00:00'),
('3A5350AB-011B-41CA-A9F0-A7B2289AE70C', 'DDDA597C-2D47-4C64-914D-6DF694AA3606', 'D612A00E-4DDD-40C2-AF86-E16C43DDA678', '8E6E5EAC-2176-4F0B-9C3D-BADAEF969807', N'Mark Thompson', '1996-08-02', N'Male', N'+201042858325', N'mark.thompson44@yahoo.com', N'الدوحة، قطر', N'بحث عبر الإنترنت', N'قلق اجتماعي (Social anxiety)', N'الزوجة - كريم حلمي - +201555016410', N'تاريخ عائلي للإصابة بالاكتئاب.', N'https://i.pravatar.cc/300?img=4', N'Active', '2025-11-26 12:00:00', '2025-11-26 12:00:00'),
('E07477FB-C7F6-4846-A2AB-0ED1F6DCB7AC', '899B50C5-FFCF-4E6E-964D-3A59908E422E', '7A3B78EC-390F-435C-BBD3-B8E4DC10DDB3', 'E0FFDBC2-F09B-4996-88E4-9F4B23D6F0FF', N'خالد فؤاد', '1976-01-10', N'Male', N'+201248158407', N'khaled.42@gmail.com', N'مدينة نصر، القاهرة', N'إحالة من طبيب عام', N'نوبات هلع (Panic attacks)', N'الأم - سارة سعيد - +201076785529', N'تاريخ عائلي للإصابة بالاكتئاب.', N'https://i.pravatar.cc/300?img=5', N'Active', '2025-09-23 12:00:00', '2025-09-23 12:00:00'),
('DC19C765-09E1-4B24-808B-8EFC97E63589', 'E73DBDDC-8CD8-42F3-A385-E5D24710E59F', 'D612A00E-4DDD-40C2-AF86-E16C43DDA678', '72F60FBA-CE9B-41DC-B960-1EAC6D43143D', N'ريهام فؤاد', '1962-04-07', N'Female', N'+201561843847', N'reham.99@hotmail.com', N'المعادي، القاهرة', N'إحالة من طبيب عام', N'إرهاق نفسي وظيفي (Burnout)', N'الأخت - ليلى جمال - +201569246529', N'تلقى/تلقت علاجاً نفسياً لمدة 6 أشهر في عام 2023.', N'https://i.pravatar.cc/300?img=6', N'Active', '2026-03-07 12:00:00', '2026-03-07 12:00:00'),
('8F8DC8EB-DCBE-44D2-B913-73C2CE8E75B3', '770DD106-350E-40A8-B1BC-4CD0BED58C40', '7A3B78EC-390F-435C-BBD3-B8E4DC10DDB3', '8F464A15-DAAB-4952-BF75-064FCFD765E7', N'ريهام جمال', '1996-06-06', N'Female', N'+201263559614', N'reham.92@yahoo.com', N'مصر الجديدة، القاهرة', N'توصية من صديق', N'قلق خفيف (Mild anxiety)', N'الأم - منى صبحي - +201185896580', N'تلقى/تلقت علاجاً نفسياً لمدة 6 أشهر في عام 2023.', N'https://i.pravatar.cc/300?img=7', N'Active', '2026-01-20 12:00:00', '2026-01-20 12:00:00'),
('F9DD6265-AFC8-44EC-8985-D0E90571FCA6', '9F442928-B53E-4FDC-B241-988969B52203', 'D612A00E-4DDD-40C2-AF86-E16C43DDA678', 'A4D9297C-F020-4C75-8F70-207DD99016FE', N'Daniel Moore', '1977-05-11', N'Male', N'+201128549977', N'daniel.moore82@gmail.com', N'الشيخ زايد، الجيزة', N'إحالة من معالج آخر', N'مؤشرات اضطراب ما بعد الصدمة (PTSD indicators)', N'الأب - أحمد حسين - +201246610402', N'لا يوجد تاريخ مرضي نفسي سابق.', N'https://i.pravatar.cc/300?img=8', N'Active', '2026-02-02 12:00:00', '2026-02-02 12:00:00'),
('A8BC8AD9-EC06-4493-8502-F88E2C965073', '6251ABB6-B294-48CA-9B75-4E959CFA04A7', '7A3B78EC-390F-435C-BBD3-B8E4DC10DDB3', 'DB091DDD-6138-42BD-B1BD-F8D5DCF6DF96', N'دينا نصار', '1974-11-07', N'Female', N'+201268330554', N'dina.46@yahoo.com', N'الإسكندرية', N'وسائل التواصل الاجتماعي', N'قلق اجتماعي (Social anxiety)', N'الابن - ليلى رضوان - +201293499517', N'استخدام سابق لأدوية مضادة للقلق تحت إشراف طبيب نفسي.', N'https://i.pravatar.cc/300?img=9', N'Active', '2026-06-16 12:00:00', '2026-06-16 12:00:00'),
('2557FC9B-7599-4EB3-9FA4-7DCD1EAE6EDA', '0D30427F-E161-441E-AA05-F6749620088C', 'D612A00E-4DDD-40C2-AF86-E16C43DDA678', '65558633-FEE9-48B7-8AF8-40BFD220B851', N'Sarah Davis', '1983-02-07', N'Female', N'+201291618695', N'sarah.davis72@gmail.com', N'الإسكندرية', N'وسائل التواصل الاجتماعي', N'قلق خفيف (Mild anxiety)', N'الأم - كريم راشد - +201160385545', N'تلقى/تلقت علاجاً نفسياً لمدة 6 أشهر في عام 2023.', N'https://i.pravatar.cc/300?img=10', N'Active', '2026-02-21 12:00:00', '2026-02-21 12:00:00'),
('87C46097-7D04-4AD1-89C2-13967E95301F', '25A9DCC1-6815-47F4-86AD-D25FF47B7ADE', '7A3B78EC-390F-435C-BBD3-B8E4DC10DDB3', '82C584D7-9870-4182-8752-2AB1097712D3', N'ملك جمال', '1984-12-22', N'Female', N'+201088474464', N'malak.85@outlook.com', N'الإسكندرية', N'إحالة من طبيب عام', N'قلق خفيف (Mild anxiety)', N'الأب - أحمد زكي - +201178455690', N'تاريخ من اضطرابات النوم منذ المراهقة.', N'https://i.pravatar.cc/300?img=11', N'Active', '2026-03-28 12:00:00', '2026-03-28 12:00:00'),
('0B875E9D-0EE5-40EF-9CC6-451259EEADB3', 'DDDA597C-2D47-4C64-914D-6DF694AA3606', 'D612A00E-4DDD-40C2-AF86-E16C43DDA678', 'AECAB352-8D01-4258-9451-A98DA466761F', N'باسم رضوان', '1961-03-18', N'Male', N'+201161263999', N'basem.51@outlook.com', N'مصر الجديدة، القاهرة', N'إحالة من طبيب عام', N'قلق خفيف (Mild anxiety)', N'الأخت - سارة فؤاد - +201273209058', N'تاريخ من اضطرابات النوم منذ المراهقة.', N'https://i.pravatar.cc/300?img=12', N'Active', '2026-01-16 12:00:00', '2026-01-16 12:00:00'),
('ED4A6B23-9515-4C85-9110-1C4CC21FAA9E', '899B50C5-FFCF-4E6E-964D-3A59908E422E', '7A3B78EC-390F-435C-BBD3-B8E4DC10DDB3', 'ED2A4F7A-6392-466E-9979-51CD2AC1FE2E', N'سلمى نصار', '1967-07-03', N'Female', N'+201546897709', N'salma.48@hotmail.com', N'مدينة نصر، القاهرة', N'وسائل التواصل الاجتماعي', N'قلق اجتماعي (Social anxiety)', N'الزوج - منى سعيد - +201552122701', N'لا يوجد تاريخ مرضي نفسي سابق.', N'https://i.pravatar.cc/300?img=13', N'Active', '2026-01-10 12:00:00', '2026-01-10 12:00:00'),
('69F104EE-9B40-40CF-A029-EC2A532693FA', 'E73DBDDC-8CD8-42F3-A385-E5D24710E59F', 'D612A00E-4DDD-40C2-AF86-E16C43DDA678', 'A45399A0-E659-442B-8C4A-F0378C9E2347', N'Mark Thompson', '1981-03-24', N'Male', N'+201140295604', N'mark.thompson27@gmail.com', N'دبي، الإمارات', N'إحالة من معالج آخر', N'اضطراب النوم (Sleep disorder)', N'الزوجة - أحمد فهمي - +201014690277', N'استخدام سابق لأدوية مضادة للقلق تحت إشراف طبيب نفسي.', N'https://i.pravatar.cc/300?img=14', N'Active', '2026-04-08 12:00:00', '2026-04-08 12:00:00'),
('56A8CAE5-A2FA-4149-B2D0-3E5B35F37D3B', '770DD106-350E-40A8-B1BC-4CD0BED58C40', '7A3B78EC-390F-435C-BBD3-B8E4DC10DDB3', '3E18E043-8106-4F93-A0C5-8BFECBF20132', N'ياسر سعيد', '1989-01-01', N'Male', N'+201524966008', N'yasser.61@yahoo.com', N'الدوحة، قطر', N'إحالة من طبيب عام', N'قلق خفيف (Mild anxiety)', N'الأم - سارة كامل - +201576748274', N'تاريخ عائلي للإصابة بالاكتئاب.', N'https://i.pravatar.cc/300?img=15', N'Active', '2026-04-28 12:00:00', '2026-04-28 12:00:00'),
('85B46DDD-B1A6-4148-BB18-D7DB4172EAF2', '9F442928-B53E-4FDC-B241-988969B52203', 'D612A00E-4DDD-40C2-AF86-E16C43DDA678', 'A7CCD1A6-1AEF-426F-A1D4-DAAA45EEFCD8', N'طارق محمود', '2006-10-02', N'Male', N'+201582312897', N'tarek.82@gmail.com', N'الدوحة، قطر', N'وسائل التواصل الاجتماعي', N'اضطراب النوم (Sleep disorder)', N'الأخ - ليلى محمود - +201092413092', N'تاريخ من اضطرابات النوم منذ المراهقة.', N'https://i.pravatar.cc/300?img=16', N'Active', '2025-09-09 12:00:00', '2025-09-09 12:00:00'),
('D5AF7A7D-26C9-4CD6-854B-11C4F6A65218', '6251ABB6-B294-48CA-9B75-4E959CFA04A7', '7A3B78EC-390F-435C-BBD3-B8E4DC10DDB3', '250B7FE2-5C94-432C-89B9-80AF87185FC0', N'Jessica Taylor', '2007-03-30', N'Female', N'+201165324096', N'jessica.taylor38@gmail.com', N'الدوحة، قطر', N'بحث عبر الإنترنت', N'مؤشرات اضطراب ما بعد الصدمة (PTSD indicators)', N'الزوج - كريم زكي - +201161999422', N'تاريخ عائلي للإصابة بالاكتئاب.', N'https://i.pravatar.cc/300?img=17', N'Active', '2025-09-30 12:00:00', '2025-09-30 12:00:00'),
('AEB38627-5539-4776-8AEC-69F91DD159F1', '0D30427F-E161-441E-AA05-F6749620088C', 'D612A00E-4DDD-40C2-AF86-E16C43DDA678', '40AF507D-2E94-40AA-9268-B0BBA290493B', N'John Carter', '1971-09-05', N'Male', N'+201164721747', N'john.carter82@gmail.com', N'6 أكتوبر، الجيزة', N'بحث عبر الإنترنت', N'أعراض اكتئابية (Depression symptoms)', N'صديق مقرب - أحمد عبدالله - +201545433167', N'تاريخ من اضطرابات النوم منذ المراهقة.', N'https://i.pravatar.cc/300?img=18', N'Active', '2025-11-13 12:00:00', '2025-11-13 12:00:00'),
('00CC925F-9671-4970-ADC6-5C26F8FBF8E5', '25A9DCC1-6815-47F4-86AD-D25FF47B7ADE', '7A3B78EC-390F-435C-BBD3-B8E4DC10DDB3', 'E64A2B73-C807-421A-BBDE-A9FCA7D29587', N'عمر زكي', '2003-12-17', N'Male', N'+201578275478', N'omar.17@yahoo.com', N'الإسكندرية', N'بحث عبر الإنترنت', N'ضغط العمل (Work stress)', N'الزوج - أحمد نصار - +201120779428', N'لا يوجد تاريخ مرضي نفسي سابق.', N'https://i.pravatar.cc/300?img=19', N'Active', '2026-04-01 12:00:00', '2026-04-01 12:00:00'),
('5947B1E9-EACB-4CD1-A014-69DBDA364362', 'DDDA597C-2D47-4C64-914D-6DF694AA3606', 'D612A00E-4DDD-40C2-AF86-E16C43DDA678', 'B672F83A-8C74-4251-BB3F-AF46E9017E52', N'أيمن الشريف', '2006-07-20', N'Male', N'+201073878131', N'ayman.50@yahoo.com', N'6 أكتوبر، الجيزة', N'توصية من صديق', N'اضطراب النوم (Sleep disorder)', N'الأم - ليلى رضوان - +201250393571', N'تلقى/تلقت علاجاً نفسياً لمدة 6 أشهر في عام 2023.', N'https://i.pravatar.cc/300?img=20', N'Active', '2025-12-07 12:00:00', '2025-12-07 12:00:00'),
('D05B32B7-0D94-4041-91CE-8952FF6C525A', '899B50C5-FFCF-4E6E-964D-3A59908E422E', '7A3B78EC-390F-435C-BBD3-B8E4DC10DDB3', 'C16CE25F-85AE-4B1D-BDF1-05FCAAE4FE41', N'ملك الشريف', '1991-09-04', N'Female', N'+201558632414', N'malak.32@yahoo.com', N'دبي، الإمارات', N'إحالة من طبيب عام', N'مؤشرات اضطراب ما بعد الصدمة (PTSD indicators)', N'الزوجة - أحمد جمال - +201014397562', N'تاريخ عائلي للإصابة بالاكتئاب.', N'https://i.pravatar.cc/300?img=21', N'Active', '2025-07-17 12:00:00', '2025-07-17 12:00:00'),
('7F9C808B-66FB-44EC-AE2F-D987CCB0D98A', 'E73DBDDC-8CD8-42F3-A385-E5D24710E59F', 'D612A00E-4DDD-40C2-AF86-E16C43DDA678', 'C7A4E929-6F01-4043-B3F3-D1A01505C894', N'منى فهمي', '2006-07-19', N'Female', N'+201171799609', N'mona.79@gmail.com', N'مصر الجديدة، القاهرة', N'بحث عبر الإنترنت', N'إرهاق نفسي وظيفي (Burnout)', N'الابن - سارة عبدالله - +201544488161', N'يعاني/تعاني من ارتفاع ضغط الدم ويأخذ/تأخذ علاجاً منتظماً.', N'https://i.pravatar.cc/300?img=22', N'Active', '2026-02-19 12:00:00', '2026-02-19 12:00:00'),
('080C5383-4556-405D-BBEC-38C843811EBD', '770DD106-350E-40A8-B1BC-4CD0BED58C40', '7A3B78EC-390F-435C-BBD3-B8E4DC10DDB3', '6F7A2849-8BFC-45CB-83E5-F305BF4B7ADF', N'Robert Anderson', '2002-01-25', N'Male', N'+201141671508', N'robert.anderson37@gmail.com', N'الدوحة، قطر', N'إحالة من معالج آخر', N'أعراض اكتئابية (Depression symptoms)', N'الزوج - أحمد زكي - +201062533829', N'لا توجد أمراض جسدية مزمنة.', N'https://i.pravatar.cc/300?img=23', N'Active', '2026-06-28 12:00:00', '2026-06-28 12:00:00'),
('5A0DA59E-4AE0-4CF6-B7CD-5BDB2F91D5E7', '9F442928-B53E-4FDC-B241-988969B52203', 'D612A00E-4DDD-40C2-AF86-E16C43DDA678', '43FC9F53-CC9C-4DCF-8670-68CA91084378', N'مريم رضوان', '1997-06-17', N'Female', N'+201091726438', N'mariam.48@outlook.com', N'مصر الجديدة، القاهرة', N'بحث عبر الإنترنت', N'ضغط العمل (Work stress)', N'الابن - كريم حلمي - +201292329638', N'لا يوجد تاريخ مرضي نفسي سابق.', N'https://i.pravatar.cc/300?img=24', N'Active', '2026-01-26 12:00:00', '2026-01-26 12:00:00'),
('0004E60D-C638-4639-BCFA-0AD58724C84E', '6251ABB6-B294-48CA-9B75-4E959CFA04A7', '7A3B78EC-390F-435C-BBD3-B8E4DC10DDB3', '4F2CF45C-16B5-4BB6-85BE-3F63C6D1091C', N'أيمن حسين', '1962-01-25', N'Male', N'+201282161213', N'ayman.25@gmail.com', N'الرياض، السعودية', N'إحالة من جهة العمل', N'قلق اجتماعي (Social anxiety)', N'صديق مقرب - كريم فؤاد - +201084225284', N'استخدام سابق لأدوية مضادة للقلق تحت إشراف طبيب نفسي.', N'https://i.pravatar.cc/300?img=25', N'Active', '2026-06-20 12:00:00', '2026-06-20 12:00:00'),
('81B640AC-DBBD-4F29-831E-F36B5C6A7580', '0D30427F-E161-441E-AA05-F6749620088C', 'D612A00E-4DDD-40C2-AF86-E16C43DDA678', 'C652B582-2B56-43AC-ABF6-A78D796BBD93', N'عمر سعيد', '1977-03-05', N'Male', N'+201054720957', N'omar.55@yahoo.com', N'مصر الجديدة، القاهرة', N'إحالة من طبيب عام', N'أعراض اكتئابية (Depression symptoms)', N'الأخت - منى رضوان - +201189042360', N'لا توجد أمراض جسدية مزمنة.', N'https://i.pravatar.cc/300?img=26', N'Active', '2026-03-07 12:00:00', '2026-03-07 12:00:00'),
('85C8A7BC-BBE9-4D03-B8D2-89AFEE304102', '25A9DCC1-6815-47F4-86AD-D25FF47B7ADE', '7A3B78EC-390F-435C-BBD3-B8E4DC10DDB3', '9F374DC9-631C-4155-B308-FA58C6FDB75B', N'فاطمة فهمي', '1973-11-13', N'Female', N'+201259453439', N'fatma.78@yahoo.com', N'الرياض، السعودية', N'إحالة من معالج آخر', N'ضغط العمل (Work stress)', N'الزوجة - أحمد حسين - +201230966083', N'لا توجد أمراض جسدية مزمنة.', N'https://i.pravatar.cc/300?img=27', N'Active', '2026-02-08 12:00:00', '2026-02-08 12:00:00'),
('46ACC094-18A3-4CCF-80C4-95D298EF4031', 'DDDA597C-2D47-4C64-914D-6DF694AA3606', 'D612A00E-4DDD-40C2-AF86-E16C43DDA678', '1C257B5A-D9E6-410F-9BAC-1744E71E1FE7', N'Robert Anderson', '1991-02-05', N'Male', N'+201581352966', N'robert.anderson19@outlook.com', N'دبي، الإمارات', N'إحالة من طبيب عام', N'إرهاق نفسي وظيفي (Burnout)', N'الأخ - ليلى نصار - +201222815802', N'لا يوجد تاريخ مرضي نفسي سابق.', N'https://i.pravatar.cc/300?img=28', N'Active', '2026-02-20 12:00:00', '2026-02-20 12:00:00'),
('F300A791-B73E-4A86-90EA-4D1EE67BBE48', '899B50C5-FFCF-4E6E-964D-3A59908E422E', '7A3B78EC-390F-435C-BBD3-B8E4DC10DDB3', 'F7F7FD61-459C-402D-A380-067F51857677', N'James Smith', '1999-02-14', N'Male', N'+201514632231', N'james.smith87@yahoo.com', N'الرياض، السعودية', N'توصية من صديق', N'قلق اجتماعي (Social anxiety)', N'الابنة - ليلى رضوان - +201155707744', N'لا يوجد تاريخ مرضي نفسي سابق.', N'https://i.pravatar.cc/300?img=29', N'Active', '2026-06-02 12:00:00', '2026-06-02 12:00:00'),
('74FDAC62-9C1A-472B-AA93-BCDC4103514D', 'E73DBDDC-8CD8-42F3-A385-E5D24710E59F', 'D612A00E-4DDD-40C2-AF86-E16C43DDA678', '9F654540-0CB3-4374-AACF-0E72E6957C1E', N'ماجد الشريف', '1996-06-15', N'Male', N'+201251123456', N'maged.29@yahoo.com', N'6 أكتوبر، الجيزة', N'بحث عبر الإنترنت', N'نوبات هلع (Panic attacks)', N'الأم - سارة راشد - +201089338359', N'لا توجد أمراض جسدية مزمنة.', N'https://i.pravatar.cc/300?img=30', N'Active', '2026-01-21 12:00:00', '2026-01-21 12:00:00'),
('5A2EBBD8-21B9-47E7-881C-B09F16F70FE6', '770DD106-350E-40A8-B1BC-4CD0BED58C40', '7A3B78EC-390F-435C-BBD3-B8E4DC10DDB3', '1B4874E3-F13E-4B41-96F2-A4DF4E747070', N'ياسر زكي', '1993-07-05', N'Male', N'+201197542449', N'yasser.99@yahoo.com', N'الرياض، السعودية', N'إحالة من جهة العمل', N'أعراض اكتئابية (Depression symptoms)', N'الابن - أحمد فؤاد - +201534262581', N'تاريخ عائلي للإصابة بالاكتئاب.', N'https://i.pravatar.cc/300?img=31', N'Active', '2026-06-02 12:00:00', '2026-06-02 12:00:00'),
('CC37E596-7078-4E46-938E-D6B59CA67B02', '9F442928-B53E-4FDC-B241-988969B52203', 'D612A00E-4DDD-40C2-AF86-E16C43DDA678', '22518D39-E6EC-4742-82D4-A9A14410326C', N'وليد زكي', '1994-09-25', N'Male', N'+201271092059', N'waleed.31@hotmail.com', N'المعادي، القاهرة', N'توصية من صديق', N'قلق خفيف (Mild anxiety)', N'الأب - منى رضوان - +201550336549', N'لا يوجد تاريخ مرضي نفسي سابق.', N'https://i.pravatar.cc/300?img=32', N'Active', '2026-03-12 12:00:00', '2026-03-12 12:00:00'),
('5F6B076C-526F-4F65-B5E3-F416520D1841', '6251ABB6-B294-48CA-9B75-4E959CFA04A7', '7A3B78EC-390F-435C-BBD3-B8E4DC10DDB3', '38E2BAFB-3323-4DCB-83E0-192A6E91F5DD', N'رامي عبدالله', '2004-12-02', N'Male', N'+201593233117', N'ramy.52@hotmail.com', N'الشيخ زايد، الجيزة', N'إحالة من طبيب عام', N'نوبات هلع (Panic attacks)', N'الأم - منى محمود - +201151378526', N'استخدام سابق لأدوية مضادة للقلق تحت إشراف طبيب نفسي.', N'https://i.pravatar.cc/300?img=33', N'Inactive', '2026-03-07 12:00:00', '2026-03-07 12:00:00'),
('8BDD5425-225E-4657-AF47-CA87A9625781', '0D30427F-E161-441E-AA05-F6749620088C', 'D612A00E-4DDD-40C2-AF86-E16C43DDA678', '8C82ABCC-10FE-487B-9F8F-1E502FF589FD', N'أمل فؤاد', '1979-09-04', N'Female', N'+201134132426', N'amal.32@outlook.com', N'دبي، الإمارات', N'إحالة من جهة العمل', N'نوبات هلع (Panic attacks)', N'الزوجة - أحمد الشريف - +201157105583', N'تاريخ من اضطرابات النوم منذ المراهقة.', N'https://i.pravatar.cc/300?img=34', N'Inactive', '2025-11-18 12:00:00', '2025-11-18 12:00:00'),
('D149A229-6389-48F8-91B0-BE1D2E2900B1', '25A9DCC1-6815-47F4-86AD-D25FF47B7ADE', '7A3B78EC-390F-435C-BBD3-B8E4DC10DDB3', '9CED10E6-0BFB-4D43-9656-019687D99BC4', N'James Smith', '1975-12-13', N'Male', N'+201053887732', N'james.smith72@outlook.com', N'مصر الجديدة، القاهرة', N'إحالة من جهة العمل', N'قلق اجتماعي (Social anxiety)', N'الأب - ليلى فهمي - +201565688483', N'استخدام سابق لأدوية مضادة للقلق تحت إشراف طبيب نفسي.', N'https://i.pravatar.cc/300?img=35', N'Discharged', '2025-09-03 12:00:00', '2025-09-03 12:00:00');

-- ===== Sessions =====
INSERT INTO [Sessions] ([Id], [PatientId], [IntakeFormId], [SessionNumber], [SessionDate], [DurationMinutes], [SessionType], [Status], [CreatedAt], [UpdatedAt])
VALUES
('A7B3EE02-C9FB-4BBF-9010-DECC6A78AFEA', '692BFE83-C7BF-4FAA-9245-0EEDE315EBDB', NULL, 1, '2026-07-05', 60, N'Individual', N'Completed', '2026-07-05 12:00:00', '2026-07-05 12:00:00'),
('7D92D827-F166-491C-917A-2C79FDCDEC89', '692BFE83-C7BF-4FAA-9245-0EEDE315EBDB', NULL, 2, '2026-07-06', 60, N'Individual', N'Completed', '2026-07-06 12:00:00', '2026-07-06 12:00:00'),
('76683417-7C3F-4FCE-826E-D6F38E4F2B4A', '692BFE83-C7BF-4FAA-9245-0EEDE315EBDB', NULL, 3, '2026-07-03', NULL, N'Individual', N'Cancelled', '2026-07-03 12:00:00', '2026-07-03 12:00:00'),
('04A0B928-7AB9-43F5-899C-564EA121E413', '692BFE83-C7BF-4FAA-9245-0EEDE315EBDB', NULL, 4, '2026-07-13', 60, N'Video', N'Scheduled', '2026-07-13 12:00:00', '2026-07-13 12:00:00'),
('10A4AB5D-6221-4F2B-A66C-5C42A4FE8824', '5912AA70-D127-4B2A-95B6-62222A464244', NULL, 1, '2026-06-10', 45, N'Individual', N'Completed', '2026-06-10 12:00:00', '2026-06-10 12:00:00'),
('F1CB7112-4755-42CD-8D4C-5857A8B4E8E5', '5912AA70-D127-4B2A-95B6-62222A464244', NULL, 2, '2026-07-04', 60, N'Individual', N'Completed', '2026-07-04 12:00:00', '2026-07-04 12:00:00'),
('B15EABBB-411C-4BD7-8AC1-FEC235FEFDD2', 'D091A7F4-A73A-4CCE-920E-0AB7D4C36536', NULL, 1, '2025-12-31', 60, N'Individual', N'Completed', '2025-12-31 12:00:00', '2025-12-31 12:00:00'),
('096C066B-9419-418F-8903-D51212C174AD', 'D091A7F4-A73A-4CCE-920E-0AB7D4C36536', NULL, 2, '2026-07-02', 45, N'Individual', N'Completed', '2026-07-02 12:00:00', '2026-07-02 12:00:00'),
('0C04FF98-9332-446D-A019-E73ADD05CF94', '3A5350AB-011B-41CA-A9F0-A7B2289AE70C', NULL, 1, '2026-03-12', NULL, N'Individual', N'Cancelled', '2026-03-12 12:00:00', '2026-03-12 12:00:00'),
('4C70BA40-6139-40B7-82A6-591F4A6FAC29', '3A5350AB-011B-41CA-A9F0-A7B2289AE70C', NULL, 2, '2026-07-01', 45, N'Individual', N'Completed', '2026-07-01 12:00:00', '2026-07-01 12:00:00'),
('FD79A60D-42E5-4A7D-AFA9-BFAEF45CC0E7', 'E07477FB-C7F6-4846-A2AB-0ED1F6DCB7AC', NULL, 1, '2025-12-24', 45, N'Individual', N'Completed', '2025-12-24 12:00:00', '2025-12-24 12:00:00'),
('9208D4AC-6CA4-4A0A-B49F-84D8D3206AE2', 'E07477FB-C7F6-4846-A2AB-0ED1F6DCB7AC', NULL, 2, '2026-04-02', NULL, N'Individual', N'Cancelled', '2026-04-02 12:00:00', '2026-04-02 12:00:00'),
('46A61C68-DD9D-42E3-A38C-5F5C6880335A', 'E07477FB-C7F6-4846-A2AB-0ED1F6DCB7AC', NULL, 3, '2026-07-06', 45, N'Video', N'Completed', '2026-07-06 12:00:00', '2026-07-06 12:00:00'),
('6AE8B40D-B31E-4AC4-9191-E7E7CF469600', 'DC19C765-09E1-4B24-808B-8EFC97E63589', NULL, 1, '2026-05-03', NULL, N'Individual', N'Cancelled', '2026-05-03 12:00:00', '2026-05-03 12:00:00'),
('08C73328-17D8-4F68-9E83-D0C875DA6203', 'DC19C765-09E1-4B24-808B-8EFC97E63589', NULL, 2, '2026-07-05', NULL, N'Video', N'Cancelled', '2026-07-05 12:00:00', '2026-07-05 12:00:00'),
('2D1443E1-0764-4DD0-8D24-B721BFA615A1', '8F8DC8EB-DCBE-44D2-B913-73C2CE8E75B3', NULL, 1, '2026-03-01', 60, N'Video', N'Completed', '2026-03-01 12:00:00', '2026-03-01 12:00:00'),
('4366AB54-889D-4D04-84CB-787836045D01', '8F8DC8EB-DCBE-44D2-B913-73C2CE8E75B3', NULL, 2, '2026-04-14', NULL, N'Individual', N'Cancelled', '2026-04-14 12:00:00', '2026-04-14 12:00:00'),
('4AC29786-DCC5-4AE9-8AC7-726C581FAF1B', '8F8DC8EB-DCBE-44D2-B913-73C2CE8E75B3', NULL, 3, '2026-05-21', 60, N'Individual', N'Completed', '2026-05-21 12:00:00', '2026-05-21 12:00:00'),
('F1E9EA4B-AD25-433C-8747-BBBDB53B9451', '8F8DC8EB-DCBE-44D2-B913-73C2CE8E75B3', NULL, 4, '2026-07-01', 50, N'Individual', N'Completed', '2026-07-01 12:00:00', '2026-07-01 12:00:00'),
('E5DD9382-7B57-4241-99B1-A13DEA9F5AC5', 'F9DD6265-AFC8-44EC-8985-D0E90571FCA6', NULL, 1, '2026-03-22', 50, N'Video', N'Completed', '2026-03-22 12:00:00', '2026-03-22 12:00:00'),
('F0D8515B-410C-4BEB-B7AA-671C5F6CAE8E', 'F9DD6265-AFC8-44EC-8985-D0E90571FCA6', NULL, 2, '2026-05-11', 45, N'Individual', N'Completed', '2026-05-11 12:00:00', '2026-05-11 12:00:00'),
('0E5A0801-BD35-4481-97E3-E527BE286E5B', 'F9DD6265-AFC8-44EC-8985-D0E90571FCA6', NULL, 3, '2026-07-07', 45, N'Video', N'Scheduled', '2026-07-07 12:00:00', '2026-07-07 12:00:00'),
('27DA408C-F4D6-4735-B4BB-E746F3D03036', 'A8BC8AD9-EC06-4493-8502-F88E2C965073', NULL, 1, '2026-06-21', 45, N'Individual', N'Completed', '2026-06-21 12:00:00', '2026-06-21 12:00:00'),
('3525A4B4-DD39-4244-AF83-16C8EE2945DE', 'A8BC8AD9-EC06-4493-8502-F88E2C965073', NULL, 2, '2026-07-02', NULL, N'Video', N'Cancelled', '2026-07-02 12:00:00', '2026-07-02 12:00:00'),
('36C38480-24AE-4BA1-BF4E-D214085202B2', '2557FC9B-7599-4EB3-9FA4-7DCD1EAE6EDA', NULL, 1, '2026-04-03', 60, N'Individual', N'Completed', '2026-04-03 12:00:00', '2026-04-03 12:00:00'),
('05A8490D-0183-40FE-945B-778D5E25C95B', '2557FC9B-7599-4EB3-9FA4-7DCD1EAE6EDA', NULL, 2, '2026-05-18', 50, N'Individual', N'Completed', '2026-05-18 12:00:00', '2026-05-18 12:00:00'),
('C3D15EB8-3777-47DC-A381-4BA5AE01AC28', '2557FC9B-7599-4EB3-9FA4-7DCD1EAE6EDA', NULL, 3, '2026-07-05', 60, N'Individual', N'Completed', '2026-07-05 12:00:00', '2026-07-05 12:00:00'),
('A3A8D4E8-3093-4CC0-BA32-385410DB4E7D', '87C46097-7D04-4AD1-89C2-13967E95301F', NULL, 1, '2026-04-18', 45, N'Individual', N'Completed', '2026-04-18 12:00:00', '2026-04-18 12:00:00'),
('A52B5072-C6BA-4CFC-960A-A1C4087E4E8D', '87C46097-7D04-4AD1-89C2-13967E95301F', NULL, 2, '2026-05-14', 45, N'Individual', N'Completed', '2026-05-14 12:00:00', '2026-05-14 12:00:00'),
('29554709-C867-474C-B88C-43893283E454', '87C46097-7D04-4AD1-89C2-13967E95301F', NULL, 3, '2026-06-11', 50, N'Individual', N'Completed', '2026-06-11 12:00:00', '2026-06-11 12:00:00'),
('1C4F32F1-CD57-411A-A0A3-86E35F707CAC', '87C46097-7D04-4AD1-89C2-13967E95301F', NULL, 4, '2026-07-25', 50, N'Individual', N'Scheduled', '2026-07-25 12:00:00', '2026-07-25 12:00:00'),
('0AE239A6-0730-4DF5-9AD0-31C720B80E5A', '0B875E9D-0EE5-40EF-9CC6-451259EEADB3', NULL, 1, '2026-04-09', 60, N'Video', N'Completed', '2026-04-09 12:00:00', '2026-04-09 12:00:00'),
('CF786960-4B27-437A-81B3-75B006A463F5', '0B875E9D-0EE5-40EF-9CC6-451259EEADB3', NULL, 2, '2026-07-12', 60, N'Individual', N'Scheduled', '2026-07-12 12:00:00', '2026-07-12 12:00:00'),
('C539C13F-D74A-4102-A252-21E5D17F9C87', 'ED4A6B23-9515-4C85-9110-1C4CC21FAA9E', NULL, 1, '2026-04-04', 45, N'Individual', N'Completed', '2026-04-04 12:00:00', '2026-04-04 12:00:00'),
('EDEEA39A-FDA6-4D99-A352-66EF2A1E4080', 'ED4A6B23-9515-4C85-9110-1C4CC21FAA9E', NULL, 2, '2026-07-17', 60, N'Individual', N'Scheduled', '2026-07-17 12:00:00', '2026-07-17 12:00:00'),
('E26DF653-F9E1-43BD-B4DA-F251898B7257', '69F104EE-9B40-40CF-A029-EC2A532693FA', NULL, 1, '2026-05-04', NULL, N'Individual', N'Cancelled', '2026-05-04 12:00:00', '2026-05-04 12:00:00'),
('85219D71-7EBF-4A85-B596-0206BCBDFA97', '69F104EE-9B40-40CF-A029-EC2A532693FA', NULL, 2, '2026-06-03', 45, N'Video', N'Completed', '2026-06-03 12:00:00', '2026-06-03 12:00:00'),
('1F48FCD0-0041-4184-A186-9B746282FB61', '69F104EE-9B40-40CF-A029-EC2A532693FA', NULL, 3, '2026-07-09', 50, N'Individual', N'Scheduled', '2026-07-09 12:00:00', '2026-07-09 12:00:00'),
('A2414079-E893-4B53-838C-9BF8AFE0D58B', '56A8CAE5-A2FA-4149-B2D0-3E5B35F37D3B', NULL, 1, '2026-05-28', 60, N'Individual', N'Completed', '2026-05-28 12:00:00', '2026-05-28 12:00:00'),
('FF1532E8-86A6-4F7B-B5ED-61740A063F13', '56A8CAE5-A2FA-4149-B2D0-3E5B35F37D3B', NULL, 2, '2026-07-02', 50, N'Individual', N'Completed', '2026-07-02 12:00:00', '2026-07-02 12:00:00'),
('0CA17981-1D9C-44C7-9C47-CE3D8D0E6DE6', '85B46DDD-B1A6-4148-BB18-D7DB4172EAF2', NULL, 1, '2026-02-02', 60, N'Individual', N'Completed', '2026-02-02 12:00:00', '2026-02-02 12:00:00'),
('C83FE51B-B90D-4C8E-B45E-7792FA259A78', '85B46DDD-B1A6-4148-BB18-D7DB4172EAF2', NULL, 2, '2026-07-02', 50, N'Video', N'Completed', '2026-07-02 12:00:00', '2026-07-02 12:00:00'),
('554FDF1D-2CD9-4BB3-867C-68CD1D3A8646', 'D5AF7A7D-26C9-4CD6-854B-11C4F6A65218', NULL, 1, '2026-02-17', 45, N'Video', N'Completed', '2026-02-17 12:00:00', '2026-02-17 12:00:00'),
('928B83B8-924D-40D3-BC4F-087A3FA2570A', 'D5AF7A7D-26C9-4CD6-854B-11C4F6A65218', NULL, 2, '2026-07-24', 45, N'Individual', N'Scheduled', '2026-07-24 12:00:00', '2026-07-24 12:00:00'),
('1C02E1DD-316B-41C9-AFC4-27F3E7A9C134', 'AEB38627-5539-4776-8AEC-69F91DD159F1', NULL, 1, '2026-03-10', 50, N'Individual', N'Completed', '2026-03-10 12:00:00', '2026-03-10 12:00:00'),
('05610285-66C2-47D2-A2DD-8845E1B88436', 'AEB38627-5539-4776-8AEC-69F91DD159F1', NULL, 2, '2026-07-02', NULL, N'Video', N'Cancelled', '2026-07-02 12:00:00', '2026-07-02 12:00:00'),
('C8654AA4-C88A-46FE-B2DC-A6C829B24BFC', '00CC925F-9671-4970-ADC6-5C26F8FBF8E5', NULL, 1, '2026-05-19', 60, N'Individual', N'Completed', '2026-05-19 12:00:00', '2026-05-19 12:00:00'),
('CF097B9F-3907-4A39-A16C-C5DD4346AF43', '00CC925F-9671-4970-ADC6-5C26F8FBF8E5', NULL, 2, '2026-07-03', NULL, N'Individual', N'Cancelled', '2026-07-03 12:00:00', '2026-07-03 12:00:00'),
('31979B1E-61F4-44E1-9B53-812779FD916D', '5947B1E9-EACB-4CD1-A014-69DBDA364362', NULL, 1, '2026-03-20', NULL, N'Video', N'Cancelled', '2026-03-20 12:00:00', '2026-03-20 12:00:00'),
('4786FE61-DDA2-4ACE-80FA-209E118D43E5', '5947B1E9-EACB-4CD1-A014-69DBDA364362', NULL, 2, '2026-07-21', 50, N'Individual', N'Scheduled', '2026-07-21 12:00:00', '2026-07-21 12:00:00'),
('C85F5EBF-E90B-401B-9150-F1D1A30FB302', 'D05B32B7-0D94-4041-91CE-8952FF6C525A', NULL, 1, '2025-11-09', 50, N'Video', N'Completed', '2025-11-09 12:00:00', '2025-11-09 12:00:00'),
('B8BBDFF9-9185-42D0-8C22-A308E4F21F57', 'D05B32B7-0D94-4041-91CE-8952FF6C525A', NULL, 2, '2026-03-09', 60, N'Video', N'Completed', '2026-03-09 12:00:00', '2026-03-09 12:00:00'),
('B5552383-E05A-41A8-8152-DAA98148BC38', 'D05B32B7-0D94-4041-91CE-8952FF6C525A', NULL, 3, '2026-07-16', 45, N'Individual', N'Scheduled', '2026-07-16 12:00:00', '2026-07-16 12:00:00'),
('315035A9-3592-4C54-B16F-A384A0063352', '7F9C808B-66FB-44EC-AE2F-D987CCB0D98A', NULL, 1, '2026-04-06', 60, N'Individual', N'Completed', '2026-04-06 12:00:00', '2026-04-06 12:00:00'),
('AE4E3748-73ED-4117-BD6D-6E67AF6A5F3C', '7F9C808B-66FB-44EC-AE2F-D987CCB0D98A', NULL, 2, '2026-05-16', 50, N'Individual', N'Completed', '2026-05-16 12:00:00', '2026-05-16 12:00:00'),
('C56F9FCF-25CF-4C1D-A2FC-D8DACA146EB4', '7F9C808B-66FB-44EC-AE2F-D987CCB0D98A', NULL, 3, '2026-07-04', 50, N'Individual', N'Completed', '2026-07-04 12:00:00', '2026-07-04 12:00:00'),
('FF597944-9B27-41B5-AF4C-BA6117E08228', '080C5383-4556-405D-BBEC-38C843811EBD', NULL, 1, '2026-06-28', 45, N'Video', N'Completed', '2026-06-28 12:00:00', '2026-06-28 12:00:00'),
('0A98C5C5-0F8F-4567-9A95-D671F77A6282', '080C5383-4556-405D-BBEC-38C843811EBD', NULL, 2, '2026-07-04', 45, N'Individual', N'Completed', '2026-07-04 12:00:00', '2026-07-04 12:00:00'),
('45B36113-F659-49C5-AB7E-89FEEBBA5B55', '080C5383-4556-405D-BBEC-38C843811EBD', NULL, 3, '2026-07-01', 50, N'Individual', N'Completed', '2026-07-01 12:00:00', '2026-07-01 12:00:00'),
('ADF591C2-5A2B-45E6-B40C-0BF50F25455C', '080C5383-4556-405D-BBEC-38C843811EBD', NULL, 4, '2026-07-17', 45, N'Individual', N'Scheduled', '2026-07-17 12:00:00', '2026-07-17 12:00:00'),
('56227A18-E296-4D00-ABB9-7208467264A0', '5A0DA59E-4AE0-4CF6-B7CD-5BDB2F91D5E7', NULL, 1, '2026-03-02', 50, N'Individual', N'Completed', '2026-03-02 12:00:00', '2026-03-02 12:00:00'),
('01A28308-2E43-49B7-9BD8-C14307882203', '5A0DA59E-4AE0-4CF6-B7CD-5BDB2F91D5E7', NULL, 2, '2026-04-13', 50, N'Individual', N'Completed', '2026-04-13 12:00:00', '2026-04-13 12:00:00'),
('499EB6D3-E004-40BA-8E14-4853C45DEC82', '5A0DA59E-4AE0-4CF6-B7CD-5BDB2F91D5E7', NULL, 3, '2026-05-27', 60, N'Individual', N'Completed', '2026-05-27 12:00:00', '2026-05-27 12:00:00'),
('FB8DD8CC-B9C1-4ECF-8B1A-2C1606A5D6BE', '5A0DA59E-4AE0-4CF6-B7CD-5BDB2F91D5E7', NULL, 4, '2026-07-22', 60, N'Video', N'Scheduled', '2026-07-22 12:00:00', '2026-07-22 12:00:00'),
('0645D2D1-65AF-453D-BE7E-4A6FF8F4E34D', '0004E60D-C638-4639-BCFA-0AD58724C84E', NULL, 1, '2026-06-24', 50, N'Individual', N'Completed', '2026-06-24 12:00:00', '2026-06-24 12:00:00'),
('E5636257-0EBF-457B-8094-022B1D64A8B8', '0004E60D-C638-4639-BCFA-0AD58724C84E', NULL, 2, '2026-07-05', 45, N'Video', N'Completed', '2026-07-05 12:00:00', '2026-07-05 12:00:00'),
('0C342091-2070-480C-B4A9-CA5C87431675', '81B640AC-DBBD-4F29-831E-F36B5C6A7580', NULL, 1, '2026-04-13', 50, N'Individual', N'Completed', '2026-04-13 12:00:00', '2026-04-13 12:00:00'),
('A2EAF665-0557-4AE0-9EFC-0B19F1BA3508', '81B640AC-DBBD-4F29-831E-F36B5C6A7580', NULL, 2, '2026-05-25', 45, N'Video', N'Completed', '2026-05-25 12:00:00', '2026-05-25 12:00:00'),
('0C8173F9-F3B1-4D88-8E0E-20E2F5CF0755', '81B640AC-DBBD-4F29-831E-F36B5C6A7580', NULL, 3, '2026-07-03', 50, N'Video', N'Completed', '2026-07-03 12:00:00', '2026-07-03 12:00:00'),
('D4C146C2-BB53-4EFB-9B94-EFACEF9164AA', '85C8A7BC-BBE9-4D03-B8D2-89AFEE304102', NULL, 1, '2026-04-21', 45, N'Individual', N'Completed', '2026-04-21 12:00:00', '2026-04-21 12:00:00'),
('BEF5F5E1-ACAF-4EA0-A5F5-A96507C6BB86', '85C8A7BC-BBE9-4D03-B8D2-89AFEE304102', NULL, 2, '2026-07-06', 50, N'Individual', N'Completed', '2026-07-06 12:00:00', '2026-07-06 12:00:00'),
('791CF984-DFC4-4356-90CD-3D8AAE4131CD', '46ACC094-18A3-4CCF-80C4-95D298EF4031', NULL, 1, '2026-04-03', NULL, N'Video', N'Cancelled', '2026-04-03 12:00:00', '2026-04-03 12:00:00'),
('780CDA59-8703-4129-97EF-3DD5B5DA81F6', '46ACC094-18A3-4CCF-80C4-95D298EF4031', NULL, 2, '2026-05-20', 45, N'Video', N'Completed', '2026-05-20 12:00:00', '2026-05-20 12:00:00'),
('9E5267FE-E295-4601-A5DA-A2F7E14B9F17', '46ACC094-18A3-4CCF-80C4-95D298EF4031', NULL, 3, '2026-07-07', 45, N'Video', N'Scheduled', '2026-07-07 12:00:00', '2026-07-07 12:00:00'),
('B3C38767-E549-405D-9511-B2CCB1C7D593', 'F300A791-B73E-4A86-90EA-4D1EE67BBE48', NULL, 1, '2026-06-13', 45, N'Individual', N'Completed', '2026-06-13 12:00:00', '2026-06-13 12:00:00'),
('8B4DF66D-564A-4E7A-B306-EE1199235349', 'F300A791-B73E-4A86-90EA-4D1EE67BBE48', NULL, 2, '2026-06-21', 60, N'Individual', N'Completed', '2026-06-21 12:00:00', '2026-06-21 12:00:00'),
('29003C60-BC71-4431-A40C-8D499B74176F', 'F300A791-B73E-4A86-90EA-4D1EE67BBE48', NULL, 3, '2026-07-07', 50, N'Individual', N'Scheduled', '2026-07-07 12:00:00', '2026-07-07 12:00:00'),
('4F38ED4C-25AE-4AB8-9A1F-2672DD48C098', '74FDAC62-9C1A-472B-AA93-BCDC4103514D', NULL, 1, '2026-03-04', 50, N'Video', N'Completed', '2026-03-04 12:00:00', '2026-03-04 12:00:00'),
('8816F55F-7BEB-49EE-9AC3-79F66AACCEA2', '74FDAC62-9C1A-472B-AA93-BCDC4103514D', NULL, 2, '2026-04-13', 60, N'Individual', N'Completed', '2026-04-13 12:00:00', '2026-04-13 12:00:00'),
('DDAE2295-84EC-4AA3-BD59-8177A1BA7857', '74FDAC62-9C1A-472B-AA93-BCDC4103514D', NULL, 3, '2026-05-24', NULL, N'Individual', N'Cancelled', '2026-05-24 12:00:00', '2026-05-24 12:00:00'),
('CE7DCAB3-36E0-4870-88AC-BFBBA0AEF41C', '74FDAC62-9C1A-472B-AA93-BCDC4103514D', NULL, 4, '2026-07-05', NULL, N'Individual', N'Cancelled', '2026-07-05 12:00:00', '2026-07-05 12:00:00'),
('19410793-2CE3-49F0-934A-06ACE65086C8', '5A2EBBD8-21B9-47E7-881C-B09F16F70FE6', NULL, 1, '2026-06-08', 50, N'Individual', N'Completed', '2026-06-08 12:00:00', '2026-06-08 12:00:00'),
('D97E930F-5B54-49AF-A79C-12BC24994007', '5A2EBBD8-21B9-47E7-881C-B09F16F70FE6', NULL, 2, '2026-06-15', 45, N'Video', N'Completed', '2026-06-15 12:00:00', '2026-06-15 12:00:00'),
('910BA1CC-6486-43F6-B1D9-29C5FA60938A', '5A2EBBD8-21B9-47E7-881C-B09F16F70FE6', NULL, 3, '2026-06-28', 60, N'Individual', N'Completed', '2026-06-28 12:00:00', '2026-06-28 12:00:00'),
('95037F97-9F2D-461F-B21F-395C39BB75B9', '5A2EBBD8-21B9-47E7-881C-B09F16F70FE6', NULL, 4, '2026-07-02', 45, N'Video', N'Completed', '2026-07-02 12:00:00', '2026-07-02 12:00:00'),
('9F9CBB05-C4FC-4357-BAB8-626831983D12', 'CC37E596-7078-4E46-938E-D6B59CA67B02', NULL, 1, '2026-04-16', 45, N'Individual', N'Completed', '2026-04-16 12:00:00', '2026-04-16 12:00:00'),
('FD48F6AD-4230-42D3-BBB6-99FD34969B65', 'CC37E596-7078-4E46-938E-D6B59CA67B02', NULL, 2, '2026-05-24', 45, N'Video', N'Completed', '2026-05-24 12:00:00', '2026-05-24 12:00:00'),
('7D73CB5F-F684-49DD-97F3-167AF15F1EA6', 'CC37E596-7078-4E46-938E-D6B59CA67B02', NULL, 3, '2026-07-01', 60, N'Individual', N'Completed', '2026-07-01 12:00:00', '2026-07-01 12:00:00'),
('4E7001AB-D318-4487-8933-454C843D9DDE', '5F6B076C-526F-4F65-B5E3-F416520D1841', NULL, 1, '2026-04-13', 60, N'Individual', N'Completed', '2026-04-13 12:00:00', '2026-04-13 12:00:00'),
('47525F0C-1C7F-4E0F-A272-1FCB7624FFF2', '5F6B076C-526F-4F65-B5E3-F416520D1841', NULL, 2, '2026-05-27', 50, N'Individual', N'Completed', '2026-05-27 12:00:00', '2026-05-27 12:00:00'),
('9C6D313D-C829-49F0-A7BC-7B31ACDF1470', '5F6B076C-526F-4F65-B5E3-F416520D1841', NULL, 3, '2026-07-01', 45, N'Individual', N'Completed', '2026-07-01 12:00:00', '2026-07-01 12:00:00'),
('0EC9D14D-D779-4494-8964-2E0F00C2F62E', '8BDD5425-225E-4657-AF47-CA87A9625781', NULL, 1, '2026-01-14', 60, N'Individual', N'Completed', '2026-01-14 12:00:00', '2026-01-14 12:00:00'),
('FD348A2A-843F-4A0F-AA8D-947D8AEE9E62', '8BDD5425-225E-4657-AF47-CA87A9625781', NULL, 2, '2026-03-10', 50, N'Individual', N'Completed', '2026-03-10 12:00:00', '2026-03-10 12:00:00'),
('9D9D68A9-B018-44C2-893D-4D6C0D48AA64', '8BDD5425-225E-4657-AF47-CA87A9625781', NULL, 3, '2026-05-05', 50, N'Video', N'Completed', '2026-05-05 12:00:00', '2026-05-05 12:00:00'),
('EABD6BAB-CF5B-4186-BB73-A7B16CB0A83D', '8BDD5425-225E-4657-AF47-CA87A9625781', NULL, 4, '2026-07-04', 50, N'Individual', N'Completed', '2026-07-04 12:00:00', '2026-07-04 12:00:00'),
('315AE4C6-91B0-4420-A5AB-E47529ADD30F', 'D149A229-6389-48F8-91B0-BE1D2E2900B1', NULL, 1, '2025-11-18', 50, N'Individual', N'Completed', '2025-11-18 12:00:00', '2025-11-18 12:00:00'),
('BF16DE28-32DB-42BE-9F5F-6F1AB0E2DAB7', 'D149A229-6389-48F8-91B0-BE1D2E2900B1', NULL, 2, '2026-02-01', 50, N'Individual', N'Completed', '2026-02-01 12:00:00', '2026-02-01 12:00:00'),
('82E317FF-BE49-4DB8-9A64-DD559A60A5E5', 'D149A229-6389-48F8-91B0-BE1D2E2900B1', NULL, 3, '2026-04-17', 50, N'Video', N'Completed', '2026-04-17 12:00:00', '2026-04-17 12:00:00'),
('1C698FC3-FF04-4C2D-BE8F-3D80972A15FA', 'D149A229-6389-48F8-91B0-BE1D2E2900B1', NULL, 4, '2026-07-01', 45, N'Video', N'Completed', '2026-07-01 12:00:00', '2026-07-01 12:00:00');

-- ===== SessionNotes =====
INSERT INTO [SessionNotes] ([Id], [SessionId], [Observations], [Interventions], [PatientResponse], [HomeworkAssigned], [NextGoals], [CreatedAt], [UpdatedAt])
VALUES
('79014DB5-3467-48BD-B4DB-6D81E0D77394', 'A7B3EE02-C9FB-4BBF-9010-DECC6A78AFEA', N'لوحظ اضطراب في نمط النوم يؤثر على المزاج العام. تم استعراض تمارين التنفس، والمريض يجد صعوبة في تطبيقها بانتظام.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة متوسطة، بحاجة لمزيد من الوقت.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-07-05 13:00:00', '2026-07-05 13:00:00'),
('2E902CB4-65B8-4CC0-A1D6-1A85799FE16B', '7D92D827-F166-491C-917A-2C79FDCDEC89', N'المريض ما زال يعاني من ضغط العمل لكنه يطور استراتيجيات تأقلم أفضل. المريض ما زال يعاني من ضغط العمل لكنه يطور استراتيجيات تأقلم أفضل.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-07-06 13:00:00', '2026-07-06 13:00:00'),
('44494D7E-A771-4B2D-84BF-508733B7EC71', '10A4AB5D-6221-4F2B-A66C-5C42A4FE8824', N'أفاد المريض بانخفاض مستوى القلق مقارنة بالأسبوع الماضي. نمط النوم بدأ في التحسن تدريجياً.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-06-10 13:00:00', '2026-06-10 13:00:00'),
('EAA07143-A486-4DAB-A8D7-74CFC87B3121', 'F1CB7112-4755-42CD-8D4C-5857A8B4E8E5', N'المريض ما زال يعاني من ضغط العمل لكنه يطور استراتيجيات تأقلم أفضل. Breathing exercises seem effective; sleep pattern improving.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-07-04 13:00:00', '2026-07-04 13:00:00'),
('598ED45B-49E3-4976-8039-CF0513C66490', 'B15EABBB-411C-4BD7-8AC1-FEC235FEFDD2', N'المريض ما زال يعاني من ضغط العمل لكنه يطور استراتيجيات تأقلم أفضل. Patient reports reduced anxiety levels compared to previous week.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2025-12-31 13:00:00', '2025-12-31 13:00:00'),
('A0AE50FD-700C-43EF-B76A-B736094E2C6E', '096C066B-9419-418F-8903-D51212C174AD', N'أفاد المريض بانخفاض مستوى القلق مقارنة بالأسبوع الماضي. المريض ما زال يعاني من ضغط العمل لكنه يطور استراتيجيات تأقلم أفضل.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-07-02 13:00:00', '2026-07-02 13:00:00'),
('7F828BF4-E23D-4AD5-8362-4F49FC60577C', '4C70BA40-6139-40B7-82A6-591F4A6FAC29', N'Patient reports reduced anxiety levels compared to previous week. Breathing exercises seem effective; sleep pattern improving.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-07-01 13:00:00', '2026-07-01 13:00:00'),
('8F9FF72C-32AE-4945-9DC2-1C174809C14C', 'FD79A60D-42E5-4A7D-AFA9-BFAEF45CC0E7', N'تم استعراض تمارين التنفس، والمريض يجد صعوبة في تطبيقها بانتظام. المريض يعاني من ضغط العمل بشكل مستمر.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة متوسطة، بحاجة لمزيد من الوقت.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2025-12-24 13:00:00', '2025-12-24 13:00:00'),
('9F61063C-3F28-4A7E-AA22-336B9A0A6D9C', '46A61C68-DD9D-42E3-A38C-5F5C6880335A', N'Patient reports reduced anxiety levels compared to previous week. المريض ما زال يعاني من ضغط العمل لكنه يطور استراتيجيات تأقلم أفضل.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-07-06 13:00:00', '2026-07-06 13:00:00'),
('66664AD9-569A-4ECC-813A-5347CE841E89', '2D1443E1-0764-4DD0-8D24-B721BFA615A1', N'لوحظ اضطراب في نمط النوم يؤثر على المزاج العام. المريض يعاني من ضغط العمل بشكل مستمر.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة متوسطة، بحاجة لمزيد من الوقت.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-03-01 13:00:00', '2026-03-01 13:00:00'),
('E8162162-56E4-413C-8057-D603886AD2FC', '4AC29786-DCC5-4AE9-8AC7-726C581FAF1B', N'Patient reports reduced anxiety levels compared to previous week. Breathing exercises seem effective; sleep pattern improving.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-05-21 13:00:00', '2026-05-21 13:00:00'),
('80B4ABB6-5FEF-4CB5-8A49-7005BD6E8DF0', 'F1E9EA4B-AD25-433C-8747-BBBDB53B9451', N'Patient reports reduced anxiety levels compared to previous week. استجابة إيجابية للتدخلات العلاجية المستخدمة خلال الجلسة.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-07-01 13:00:00', '2026-07-01 13:00:00'),
('0E8E7F46-6BC9-42C2-837E-6CA2C464F7B7', 'E5DD9382-7B57-4241-99B1-A13DEA9F5AC5', N'ما زال المريض يعاني من مستوى مرتفع من القلق خلال الجلسة. تم استعراض تمارين التنفس، والمريض يجد صعوبة في تطبيقها بانتظام.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة متوسطة، بحاجة لمزيد من الوقت.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-03-22 13:00:00', '2026-03-22 13:00:00'),
('9BFA1AE9-A380-4912-8D7A-929B8867325D', 'F0D8515B-410C-4BEB-B7AA-671C5F6CAE8E', N'استجابة إيجابية للتدخلات العلاجية المستخدمة خلال الجلسة. استجابة إيجابية للتدخلات العلاجية المستخدمة خلال الجلسة.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-05-11 13:00:00', '2026-05-11 13:00:00'),
('F3091B97-D405-4678-9216-255288DDB7BB', '27DA408C-F4D6-4735-B4BB-E746F3D03036', N'تمارين التنفس تبدو فعالة في تقليل التوتر. Patient reports reduced anxiety levels compared to previous week.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-06-21 13:00:00', '2026-06-21 13:00:00'),
('DD56B82B-C33E-48E4-B2BE-111A7C578885', '36C38480-24AE-4BA1-BF4E-D214085202B2', N'المريض ما زال يعاني من نوبات هلع متكررة خلال الأسبوع. ما زال المريض يعاني من مستوى مرتفع من القلق خلال الجلسة.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة متوسطة، بحاجة لمزيد من الوقت.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-04-03 13:00:00', '2026-04-03 13:00:00'),
('DA30E508-678E-4193-B36C-E19635A0A1A7', '05A8490D-0183-40FE-945B-778D5E25C95B', N'المريض ما زال يعاني من ضغط العمل لكنه يطور استراتيجيات تأقلم أفضل. Patient reports reduced anxiety levels compared to previous week.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-05-18 13:00:00', '2026-05-18 13:00:00'),
('8950AA13-B1BB-48E2-A48F-76AAA89D6227', 'C3D15EB8-3777-47DC-A381-4BA5AE01AC28', N'تمارين التنفس تبدو فعالة في تقليل التوتر. تمارين التنفس تبدو فعالة في تقليل التوتر.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-07-05 13:00:00', '2026-07-05 13:00:00'),
('0A839514-47F3-4D42-8366-6EF4903E0A1C', 'A3A8D4E8-3093-4CC0-BA32-385410DB4E7D', N'المريض يعاني من ضغط العمل بشكل مستمر. ما زال المريض يعاني من مستوى مرتفع من القلق خلال الجلسة.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة متوسطة، بحاجة لمزيد من الوقت.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-04-18 13:00:00', '2026-04-18 13:00:00'),
('A45700F2-3EA7-429E-BDF1-6DA740144B6B', 'A52B5072-C6BA-4CFC-960A-A1C4087E4E8D', N'تمارين التنفس تبدو فعالة في تقليل التوتر. استجابة إيجابية للتدخلات العلاجية المستخدمة خلال الجلسة.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-05-14 13:00:00', '2026-05-14 13:00:00'),
('A5143545-7DCF-4AC2-9D0F-F07FA5D9F705', '29554709-C867-474C-B88C-43893283E454', N'Breathing exercises seem effective; sleep pattern improving. Patient reports reduced anxiety levels compared to previous week.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-06-11 13:00:00', '2026-06-11 13:00:00'),
('F344BAB2-56B1-4985-BB29-66C82B0CFE60', '0AE239A6-0730-4DF5-9AD0-31C720B80E5A', N'Breathing exercises seem effective; sleep pattern improving. تمارين التنفس تبدو فعالة في تقليل التوتر.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-04-09 13:00:00', '2026-04-09 13:00:00'),
('25B599F3-DAFE-4886-BC32-9CB9F77344F4', 'C539C13F-D74A-4102-A252-21E5D17F9C87', N'Patient reports reduced anxiety levels compared to previous week. المريض ما زال يعاني من ضغط العمل لكنه يطور استراتيجيات تأقلم أفضل.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-04-04 13:00:00', '2026-04-04 13:00:00'),
('DF31F9BE-8D10-482E-A7A1-4EA465B59B8C', '85219D71-7EBF-4A85-B596-0206BCBDFA97', N'استجابة إيجابية للتدخلات العلاجية المستخدمة خلال الجلسة. أفاد المريض بانخفاض مستوى القلق مقارنة بالأسبوع الماضي.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-06-03 13:00:00', '2026-06-03 13:00:00'),
('3FA624CD-56AF-47F0-8433-E73553058B6C', 'A2414079-E893-4B53-838C-9BF8AFE0D58B', N'استجابة إيجابية للتدخلات العلاجية المستخدمة خلال الجلسة. Breathing exercises seem effective; sleep pattern improving.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-05-28 13:00:00', '2026-05-28 13:00:00'),
('F4892AFB-8D8A-42EB-B85A-29A6A531A249', 'FF1532E8-86A6-4F7B-B5ED-61740A063F13', N'استجابة إيجابية للتدخلات العلاجية المستخدمة خلال الجلسة. المريض ما زال يعاني من ضغط العمل لكنه يطور استراتيجيات تأقلم أفضل.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-07-02 13:00:00', '2026-07-02 13:00:00'),
('DDE77A48-094D-4427-B58F-E955191FBBD3', '0CA17981-1D9C-44C7-9C47-CE3D8D0E6DE6', N'استجابة إيجابية للتدخلات العلاجية المستخدمة خلال الجلسة. Patient reports reduced anxiety levels compared to previous week.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-02-02 13:00:00', '2026-02-02 13:00:00'),
('21966899-3646-49CF-9A9E-D1C4F2C17E57', 'C83FE51B-B90D-4C8E-B45E-7792FA259A78', N'تمارين التنفس تبدو فعالة في تقليل التوتر. أفاد المريض بانخفاض مستوى القلق مقارنة بالأسبوع الماضي.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-07-02 13:00:00', '2026-07-02 13:00:00'),
('B988C536-3502-4E75-8178-F94D1D7910C3', '554FDF1D-2CD9-4BB3-867C-68CD1D3A8646', N'Breathing exercises seem effective; sleep pattern improving. أفاد المريض بانخفاض مستوى القلق مقارنة بالأسبوع الماضي.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-02-17 13:00:00', '2026-02-17 13:00:00'),
('21650C4E-8CA3-4860-9387-E1830B7D4AA9', '1C02E1DD-316B-41C9-AFC4-27F3E7A9C134', N'تمارين التنفس تبدو فعالة في تقليل التوتر. تمارين التنفس تبدو فعالة في تقليل التوتر.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-03-10 13:00:00', '2026-03-10 13:00:00'),
('EA9A988C-7654-4318-A270-ED876F9DAB6F', 'C8654AA4-C88A-46FE-B2DC-A6C829B24BFC', N'المريض ما زال يعاني من ضغط العمل لكنه يطور استراتيجيات تأقلم أفضل. أفاد المريض بانخفاض مستوى القلق مقارنة بالأسبوع الماضي.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-05-19 13:00:00', '2026-05-19 13:00:00'),
('1032C5EE-9A57-49C9-B137-F86777AB82AE', 'C85F5EBF-E90B-401B-9150-F1D1A30FB302', N'المريض ما زال يعاني من نوبات هلع متكررة خلال الأسبوع. ما زال المريض يعاني من مستوى مرتفع من القلق خلال الجلسة.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة متوسطة، بحاجة لمزيد من الوقت.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2025-11-09 13:00:00', '2025-11-09 13:00:00'),
('90697B24-1D30-4552-A81C-5393B9D22E6D', 'B8BBDFF9-9185-42D0-8C22-A308E4F21F57', N'المريض ما زال يعاني من ضغط العمل لكنه يطور استراتيجيات تأقلم أفضل. المريض ما زال يعاني من ضغط العمل لكنه يطور استراتيجيات تأقلم أفضل.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-03-09 13:00:00', '2026-03-09 13:00:00'),
('0DECD8BC-87AE-402D-84D2-3B70C42EC2B6', '315035A9-3592-4C54-B16F-A384A0063352', N'تم استعراض تمارين التنفس، والمريض يجد صعوبة في تطبيقها بانتظام. ما زال المريض يعاني من مستوى مرتفع من القلق خلال الجلسة.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة متوسطة، بحاجة لمزيد من الوقت.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-04-06 13:00:00', '2026-04-06 13:00:00'),
('C1F8D056-75D7-4C3A-8A2D-62B9454E06E8', 'AE4E3748-73ED-4117-BD6D-6E67AF6A5F3C', N'أفاد المريض بانخفاض مستوى القلق مقارنة بالأسبوع الماضي. نمط النوم بدأ في التحسن تدريجياً.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-05-16 13:00:00', '2026-05-16 13:00:00'),
('3ECB5F1B-B698-4A6D-A3C6-BCE74E6668EA', 'C56F9FCF-25CF-4C1D-A2FC-D8DACA146EB4', N'Patient reports reduced anxiety levels compared to previous week. أفاد المريض بانخفاض مستوى القلق مقارنة بالأسبوع الماضي.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-07-04 13:00:00', '2026-07-04 13:00:00'),
('9C9390D4-2265-4EEF-B24D-D580337C67B2', 'FF597944-9B27-41B5-AF4C-BA6117E08228', N'تم استعراض تمارين التنفس، والمريض يجد صعوبة في تطبيقها بانتظام. لوحظ اضطراب في نمط النوم يؤثر على المزاج العام.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة متوسطة، بحاجة لمزيد من الوقت.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-06-28 13:00:00', '2026-06-28 13:00:00'),
('79832822-94BD-499B-AC82-CE2EC7A0273D', '0A98C5C5-0F8F-4567-9A95-D671F77A6282', N'استجابة إيجابية للتدخلات العلاجية المستخدمة خلال الجلسة. استجابة إيجابية للتدخلات العلاجية المستخدمة خلال الجلسة.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-07-04 13:00:00', '2026-07-04 13:00:00'),
('BB4343F9-BAE8-4886-A9C4-6A84D0ADCFBC', '45B36113-F659-49C5-AB7E-89FEEBBA5B55', N'المريض ما زال يعاني من ضغط العمل لكنه يطور استراتيجيات تأقلم أفضل. المريض ما زال يعاني من ضغط العمل لكنه يطور استراتيجيات تأقلم أفضل.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-07-01 13:00:00', '2026-07-01 13:00:00'),
('2A2B0F40-4264-4A93-BCE7-3C3DDA7FA47C', '56227A18-E296-4D00-ABB9-7208467264A0', N'ما زال المريض يعاني من مستوى مرتفع من القلق خلال الجلسة. المريض ما زال يعاني من نوبات هلع متكررة خلال الأسبوع.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة متوسطة، بحاجة لمزيد من الوقت.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-03-02 13:00:00', '2026-03-02 13:00:00'),
('E91EBB9B-106D-47CE-8ED2-3F076BD7D4A5', '01A28308-2E43-49B7-9BD8-C14307882203', N'استجابة إيجابية للتدخلات العلاجية المستخدمة خلال الجلسة. المريض ما زال يعاني من ضغط العمل لكنه يطور استراتيجيات تأقلم أفضل.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-04-13 13:00:00', '2026-04-13 13:00:00'),
('C5F3F056-2C77-4E65-8CB2-BA208511EC52', '499EB6D3-E004-40BA-8E14-4853C45DEC82', N'أفاد المريض بانخفاض مستوى القلق مقارنة بالأسبوع الماضي. Patient reports reduced anxiety levels compared to previous week.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-05-27 13:00:00', '2026-05-27 13:00:00'),
('33980FF0-DE13-4F93-8B1E-0E6AC942C011', '0645D2D1-65AF-453D-BE7E-4A6FF8F4E34D', N'Patient reports reduced anxiety levels compared to previous week. تمارين التنفس تبدو فعالة في تقليل التوتر.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-06-24 13:00:00', '2026-06-24 13:00:00'),
('782629C2-B3C3-4E1F-A99A-54F5BB2B9124', 'E5636257-0EBF-457B-8094-022B1D64A8B8', N'أفاد المريض بانخفاض مستوى القلق مقارنة بالأسبوع الماضي. المريض ما زال يعاني من ضغط العمل لكنه يطور استراتيجيات تأقلم أفضل.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-07-05 13:00:00', '2026-07-05 13:00:00'),
('DE407528-D0D9-4CC9-A3DE-5308BAFD53ED', '0C342091-2070-480C-B4A9-CA5C87431675', N'تم استعراض تمارين التنفس، والمريض يجد صعوبة في تطبيقها بانتظام. المريض يعاني من ضغط العمل بشكل مستمر.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة متوسطة، بحاجة لمزيد من الوقت.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-04-13 13:00:00', '2026-04-13 13:00:00'),
('3C4C6027-779F-4A47-9EE7-01D63AD2FEC1', 'A2EAF665-0557-4AE0-9EFC-0B19F1BA3508', N'استجابة إيجابية للتدخلات العلاجية المستخدمة خلال الجلسة. استجابة إيجابية للتدخلات العلاجية المستخدمة خلال الجلسة.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-05-25 13:00:00', '2026-05-25 13:00:00'),
('61A5EC35-6E05-4E57-80E1-3C63E03599C7', '0C8173F9-F3B1-4D88-8E0E-20E2F5CF0755', N'أفاد المريض بانخفاض مستوى القلق مقارنة بالأسبوع الماضي. Breathing exercises seem effective; sleep pattern improving.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-07-03 13:00:00', '2026-07-03 13:00:00'),
('2937A5E8-473E-40D4-B1D3-F3AD2807DCF7', 'D4C146C2-BB53-4EFB-9B94-EFACEF9164AA', N'Patient reports reduced anxiety levels compared to previous week. استجابة إيجابية للتدخلات العلاجية المستخدمة خلال الجلسة.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-04-21 13:00:00', '2026-04-21 13:00:00'),
('EE2098A2-72B7-4166-9A1D-5719F8345144', 'BEF5F5E1-ACAF-4EA0-A5F5-A96507C6BB86', N'Breathing exercises seem effective; sleep pattern improving. استجابة إيجابية للتدخلات العلاجية المستخدمة خلال الجلسة.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-07-06 13:00:00', '2026-07-06 13:00:00'),
('5A414990-7C57-4257-9CB3-0A2EB1676CC1', '780CDA59-8703-4129-97EF-3DD5B5DA81F6', N'تمارين التنفس تبدو فعالة في تقليل التوتر. أفاد المريض بانخفاض مستوى القلق مقارنة بالأسبوع الماضي.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-05-20 13:00:00', '2026-05-20 13:00:00'),
('152250B1-C85B-44D0-A60E-62C21A01B2D1', 'B3C38767-E549-405D-9511-B2CCB1C7D593', N'تم استعراض تمارين التنفس، والمريض يجد صعوبة في تطبيقها بانتظام. لوحظ اضطراب في نمط النوم يؤثر على المزاج العام.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة متوسطة، بحاجة لمزيد من الوقت.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-06-13 13:00:00', '2026-06-13 13:00:00'),
('2D24B437-6691-4BD3-9354-52F5DA118532', '8B4DF66D-564A-4E7A-B306-EE1199235349', N'المريض ما زال يعاني من ضغط العمل لكنه يطور استراتيجيات تأقلم أفضل. نمط النوم بدأ في التحسن تدريجياً.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-06-21 13:00:00', '2026-06-21 13:00:00'),
('45934888-0EA9-4238-863F-68FE87F22363', '4F38ED4C-25AE-4AB8-9A1F-2672DD48C098', N'ما زال المريض يعاني من مستوى مرتفع من القلق خلال الجلسة. ما زال المريض يعاني من مستوى مرتفع من القلق خلال الجلسة.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة متوسطة، بحاجة لمزيد من الوقت.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-03-04 13:00:00', '2026-03-04 13:00:00'),
('92158382-47F3-46FA-96EA-4CD3870634C5', '8816F55F-7BEB-49EE-9AC3-79F66AACCEA2', N'تمارين التنفس تبدو فعالة في تقليل التوتر. المريض ما زال يعاني من ضغط العمل لكنه يطور استراتيجيات تأقلم أفضل.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-04-13 13:00:00', '2026-04-13 13:00:00'),
('1760FAB4-15EB-4156-8C92-F5178FA3866A', '19410793-2CE3-49F0-934A-06ACE65086C8', N'تم استعراض تمارين التنفس، والمريض يجد صعوبة في تطبيقها بانتظام. لوحظ اضطراب في نمط النوم يؤثر على المزاج العام.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة متوسطة، بحاجة لمزيد من الوقت.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-06-08 13:00:00', '2026-06-08 13:00:00'),
('F74773D3-C53B-4F48-AC80-13FE034D0838', 'D97E930F-5B54-49AF-A79C-12BC24994007', N'المريض ما زال يعاني من ضغط العمل لكنه يطور استراتيجيات تأقلم أفضل. نمط النوم بدأ في التحسن تدريجياً.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-06-15 13:00:00', '2026-06-15 13:00:00'),
('1B6407DE-14C2-4FC8-AF5D-774DD549C327', '910BA1CC-6486-43F6-B1D9-29C5FA60938A', N'Breathing exercises seem effective; sleep pattern improving. Breathing exercises seem effective; sleep pattern improving.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-06-28 13:00:00', '2026-06-28 13:00:00'),
('B959EA6B-2FFF-4DF0-B6A0-62CA17C3CF60', '95037F97-9F2D-461F-B21F-395C39BB75B9', N'تمارين التنفس تبدو فعالة في تقليل التوتر. المريض ما زال يعاني من ضغط العمل لكنه يطور استراتيجيات تأقلم أفضل.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-07-02 13:00:00', '2026-07-02 13:00:00'),
('9772D0C0-C854-43A4-8F97-753C8A091A8B', '9F9CBB05-C4FC-4357-BAB8-626831983D12', N'المريض ما زال يعاني من نوبات هلع متكررة خلال الأسبوع. المريض يعاني من ضغط العمل بشكل مستمر.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة متوسطة، بحاجة لمزيد من الوقت.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-04-16 13:00:00', '2026-04-16 13:00:00'),
('88C60C59-8534-466A-AC2B-61462A91DFB7', 'FD48F6AD-4230-42D3-BBB6-99FD34969B65', N'تمارين التنفس تبدو فعالة في تقليل التوتر. تمارين التنفس تبدو فعالة في تقليل التوتر.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-05-24 13:00:00', '2026-05-24 13:00:00'),
('47CE2E81-EA61-4881-82DD-73C741F9B0DB', '7D73CB5F-F684-49DD-97F3-167AF15F1EA6', N'Breathing exercises seem effective; sleep pattern improving. تمارين التنفس تبدو فعالة في تقليل التوتر.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-07-01 13:00:00', '2026-07-01 13:00:00'),
('980EBBEA-2619-43E6-8553-175A7E26B5C0', '4E7001AB-D318-4487-8933-454C843D9DDE', N'لوحظ اضطراب في نمط النوم يؤثر على المزاج العام. المريض يعاني من ضغط العمل بشكل مستمر.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة متوسطة، بحاجة لمزيد من الوقت.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-04-13 13:00:00', '2026-04-13 13:00:00'),
('759AB401-5234-4395-8ECA-15286B5B2204', '47525F0C-1C7F-4E0F-A272-1FCB7624FFF2', N'المريض ما زال يعاني من ضغط العمل لكنه يطور استراتيجيات تأقلم أفضل. تمارين التنفس تبدو فعالة في تقليل التوتر.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-05-27 13:00:00', '2026-05-27 13:00:00'),
('43FAA463-0F16-4CE0-AD55-8294491D6F9C', '9C6D313D-C829-49F0-A7BC-7B31ACDF1470', N'Patient reports reduced anxiety levels compared to previous week. تمارين التنفس تبدو فعالة في تقليل التوتر.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-07-01 13:00:00', '2026-07-01 13:00:00'),
('88AAA2A2-8DF5-4A34-B9EF-426FCF8380BB', '0EC9D14D-D779-4494-8964-2E0F00C2F62E', N'تم استعراض تمارين التنفس، والمريض يجد صعوبة في تطبيقها بانتظام. المريض ما زال يعاني من نوبات هلع متكررة خلال الأسبوع.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة متوسطة، بحاجة لمزيد من الوقت.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-01-14 13:00:00', '2026-01-14 13:00:00'),
('11CAD560-665E-4A06-A00A-18485655054F', 'FD348A2A-843F-4A0F-AA8D-947D8AEE9E62', N'أفاد المريض بانخفاض مستوى القلق مقارنة بالأسبوع الماضي. Patient reports reduced anxiety levels compared to previous week.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-03-10 13:00:00', '2026-03-10 13:00:00'),
('96585C78-A4E3-4629-B451-1FA6735F1292', '9D9D68A9-B018-44C2-893D-4D6C0D48AA64', N'Breathing exercises seem effective; sleep pattern improving. أفاد المريض بانخفاض مستوى القلق مقارنة بالأسبوع الماضي.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-05-05 13:00:00', '2026-05-05 13:00:00'),
('E53BA254-828D-4316-8EF8-7DB02939C5D5', 'EABD6BAB-CF5B-4186-BB73-A7B16CB0A83D', N'أفاد المريض بانخفاض مستوى القلق مقارنة بالأسبوع الماضي. نمط النوم بدأ في التحسن تدريجياً.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-07-04 13:00:00', '2026-07-04 13:00:00'),
('570771D7-6A3D-45C7-A3AE-C5E981154BD3', '315AE4C6-91B0-4420-A5AB-E47529ADD30F', N'المريض يعاني من ضغط العمل بشكل مستمر. تم استعراض تمارين التنفس، والمريض يجد صعوبة في تطبيقها بانتظام.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة متوسطة، بحاجة لمزيد من الوقت.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2025-11-18 13:00:00', '2025-11-18 13:00:00'),
('F40E64D9-3BE4-4354-81F5-927A7D332E18', 'BF16DE28-32DB-42BE-9F5F-6F1AB0E2DAB7', N'Patient reports reduced anxiety levels compared to previous week. استجابة إيجابية للتدخلات العلاجية المستخدمة خلال الجلسة.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-02-01 13:00:00', '2026-02-01 13:00:00'),
('7A4EE744-FF45-4EB6-B9A9-E6FAF188EC1D', '82E317FF-BE49-4DB8-9A64-DD559A60A5E5', N'أفاد المريض بانخفاض مستوى القلق مقارنة بالأسبوع الماضي. Breathing exercises seem effective; sleep pattern improving.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-04-17 13:00:00', '2026-04-17 13:00:00'),
('A87F8244-2FDA-42A7-B0AD-780BD963D081', '1C698FC3-FF04-4C2D-BE8F-3D80972A15FA', N'أفاد المريض بانخفاض مستوى القلق مقارنة بالأسبوع الماضي. تمارين التنفس تبدو فعالة في تقليل التوتر.', N'مراجعة تمارين الاسترخاء والتنفس العميق، ومناقشة استراتيجيات المواجهة.', N'استجابة إيجابية وتفاعل جيد مع الجلسة.', N'ممارسة تمرين التنفس العميق يومياً لمدة 10 دقائق.', N'الاستمرار في متابعة التقدم في الجلسة القادمة.', '2026-07-01 13:00:00', '2026-07-01 13:00:00');

-- ===== Assessments =====
INSERT INTO [Assessments] ([Id], [PatientId], [SessionId], [TemplateId], [Title], [AssessmentDate], [TotalScore], [Severity], [Notes], [Status], [CreatedAt], [UpdatedAt])
VALUES
('967C15E7-1A1F-4EAD-9616-B2D6EB33DC56', '692BFE83-C7BF-4FAA-9245-0EEDE315EBDB', NULL, 'BA53959A-A392-4E40-A220-6B0FEFF1F113', N'DASS-21', '2026-07-08', 45, N'Severe', N'تسجل الدرجات مستوى مرتفعاً من الأعراض، يوصى بمتابعة عن قرب وتكثيف الجلسات.', N'Reviewed', '2026-07-08 12:00:00', '2026-07-08 12:00:00'),
('88E530CC-60B0-4300-9290-D6892A6A0FA1', '692BFE83-C7BF-4FAA-9245-0EEDE315EBDB', NULL, 'BA53959A-A392-4E40-A220-6B0FEFF1F113', N'DASS-21', '2026-07-14', 39, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Reviewed', '2026-07-14 12:00:00', '2026-07-14 12:00:00'),
('670BEBC3-889B-4539-B3A5-C216C3BB3B47', '692BFE83-C7BF-4FAA-9245-0EEDE315EBDB', NULL, 'BA53959A-A392-4E40-A220-6B0FEFF1F113', N'DASS-21', '2026-07-19', 31, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Active', '2026-07-19 12:00:00', '2026-07-19 12:00:00'),
('ADFDC9E9-4E7C-42B1-97B2-A8E2D510F90D', '5912AA70-D127-4B2A-95B6-62222A464244', NULL, 'BA53959A-A392-4E40-A220-6B0FEFF1F113', N'DASS-21', '2026-05-29', 47, N'Severe', N'تسجل الدرجات مستوى مرتفعاً من الأعراض، يوصى بمتابعة عن قرب وتكثيف الجلسات.', N'Reviewed', '2026-05-29 12:00:00', '2026-05-29 12:00:00'),
('852ED968-631D-4DA7-A77B-CBD7C826B5C3', '5912AA70-D127-4B2A-95B6-62222A464244', NULL, 'BA53959A-A392-4E40-A220-6B0FEFF1F113', N'DASS-21', '2026-06-08', 39, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Reviewed', '2026-06-08 12:00:00', '2026-06-08 12:00:00'),
('44169A6C-34B7-4DA6-92F1-8C5147F61CB2', '5912AA70-D127-4B2A-95B6-62222A464244', NULL, 'BA53959A-A392-4E40-A220-6B0FEFF1F113', N'DASS-21', '2026-06-17', 36, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Reviewed', '2026-06-17 12:00:00', '2026-06-17 12:00:00'),
('348B5DF9-1DA4-4E37-9DE5-E09C9D59385E', '5912AA70-D127-4B2A-95B6-62222A464244', NULL, 'BA53959A-A392-4E40-A220-6B0FEFF1F113', N'DASS-21', '2026-06-27', 30, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Active', '2026-06-27 12:00:00', '2026-06-27 12:00:00'),
('5722B779-F65A-472E-BCD3-E969993CCE4E', 'D091A7F4-A73A-4CCE-920E-0AB7D4C36536', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2025-10-30', 16, N'Severe', N'تسجل الدرجات مستوى مرتفعاً من الأعراض، يوصى بمتابعة عن قرب وتكثيف الجلسات.', N'Reviewed', '2025-10-30 12:00:00', '2025-10-30 12:00:00'),
('BE0F25F0-BC59-44A5-B785-BAF31CE00C70', 'D091A7F4-A73A-4CCE-920E-0AB7D4C36536', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2026-03-03', 8, N'Mild', N'الأعراض ضمن المستوى الخفيف، استجابة جيدة للتدخلات العلاجية.', N'Active', '2026-03-03 12:00:00', '2026-03-03 12:00:00'),
('7C41BDB5-D11F-4865-8938-FF06B61BF242', '3A5350AB-011B-41CA-A9F0-A7B2289AE70C', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2026-02-08', 18, N'Severe', N'تسجل الدرجات مستوى مرتفعاً من الأعراض، يوصى بمتابعة عن قرب وتكثيف الجلسات.', N'Reviewed', '2026-02-08 12:00:00', '2026-02-08 12:00:00'),
('9F1A63BF-94C8-49D8-A977-69E9E3E5AB5F', '3A5350AB-011B-41CA-A9F0-A7B2289AE70C', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2026-04-23', 8, N'Mild', N'الأعراض ضمن المستوى الخفيف، استجابة جيدة للتدخلات العلاجية.', N'Active', '2026-04-23 12:00:00', '2026-04-23 12:00:00'),
('DE53669F-D6DA-4C2C-99A2-814677FA870B', 'E07477FB-C7F6-4846-A2AB-0ED1F6DCB7AC', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2025-12-27', 15, N'Severe', N'تسجل الدرجات مستوى مرتفعاً من الأعراض، يوصى بمتابعة عن قرب وتكثيف الجلسات.', N'Reviewed', '2025-12-27 12:00:00', '2025-12-27 12:00:00'),
('CCBE4059-9789-47CA-A7B0-2115BA922E95', 'E07477FB-C7F6-4846-A2AB-0ED1F6DCB7AC', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2026-04-02', 9, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Active', '2026-04-02 12:00:00', '2026-04-02 12:00:00'),
('102824AD-7E5E-4F20-B537-76AD82D0546C', 'DC19C765-09E1-4B24-808B-8EFC97E63589', NULL, 'BA53959A-A392-4E40-A220-6B0FEFF1F113', N'DASS-21', '2026-04-06', 50, N'Severe', N'تسجل الدرجات مستوى مرتفعاً من الأعراض، يوصى بمتابعة عن قرب وتكثيف الجلسات.', N'Reviewed', '2026-04-06 12:00:00', '2026-04-06 12:00:00'),
('20D575BC-10E1-4DCC-8985-BF36B313117B', 'DC19C765-09E1-4B24-808B-8EFC97E63589', NULL, 'BA53959A-A392-4E40-A220-6B0FEFF1F113', N'DASS-21', '2026-05-07', 39, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Reviewed', '2026-05-07 12:00:00', '2026-05-07 12:00:00'),
('36CF3954-3F9A-4F82-9769-FFBDA43120E3', 'DC19C765-09E1-4B24-808B-8EFC97E63589', NULL, 'BA53959A-A392-4E40-A220-6B0FEFF1F113', N'DASS-21', '2026-06-06', 32, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Active', '2026-06-06 12:00:00', '2026-06-06 12:00:00'),
('66DD0468-198D-44D8-B780-388AB6397E51', '8F8DC8EB-DCBE-44D2-B913-73C2CE8E75B3', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2026-03-03', 12, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Reviewed', '2026-03-03 12:00:00', '2026-03-03 12:00:00'),
('2E5D4652-A033-4C6C-9D6B-954F71B0EAC7', '8F8DC8EB-DCBE-44D2-B913-73C2CE8E75B3', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2026-04-14', 10, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Reviewed', '2026-04-14 12:00:00', '2026-04-14 12:00:00'),
('221D8CD5-1D52-4ADE-BB65-2B3792228935', '8F8DC8EB-DCBE-44D2-B913-73C2CE8E75B3', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2026-05-25', 9, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Active', '2026-05-25 12:00:00', '2026-05-25 12:00:00'),
('C56E7DF2-FD05-429D-BB0B-D552A0B8B8E6', 'F9DD6265-AFC8-44EC-8985-D0E90571FCA6', NULL, 'BA53959A-A392-4E40-A220-6B0FEFF1F113', N'DASS-21', '2026-03-25', 49, N'Severe', N'تسجل الدرجات مستوى مرتفعاً من الأعراض، يوصى بمتابعة عن قرب وتكثيف الجلسات.', N'Reviewed', '2026-03-25 12:00:00', '2026-03-25 12:00:00'),
('336966A0-022A-4ECF-B78B-C9183B3595C7', 'F9DD6265-AFC8-44EC-8985-D0E90571FCA6', NULL, 'BA53959A-A392-4E40-A220-6B0FEFF1F113', N'DASS-21', '2026-05-16', 29, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Active', '2026-05-16 12:00:00', '2026-05-16 12:00:00'),
('BD1E7FA0-23C5-4B08-B698-06D93C3E9970', 'A8BC8AD9-EC06-4493-8502-F88E2C965073', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2026-06-20', 17, N'Severe', N'تسجل الدرجات مستوى مرتفعاً من الأعراض، يوصى بمتابعة عن قرب وتكثيف الجلسات.', N'Reviewed', '2026-06-20 12:00:00', '2026-06-20 12:00:00'),
('809C7BFC-0452-4FD0-BAB1-9E7455574410', 'A8BC8AD9-EC06-4493-8502-F88E2C965073', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2026-06-24', 13, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Reviewed', '2026-06-24 12:00:00', '2026-06-24 12:00:00'),
('1F7FF790-4F94-4ACE-A062-5F4352525C36', 'A8BC8AD9-EC06-4493-8502-F88E2C965073', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2026-06-29', 12, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Reviewed', '2026-06-29 12:00:00', '2026-06-29 12:00:00'),
('D0AD6935-1F32-434C-88A3-D63F9689D350', 'A8BC8AD9-EC06-4493-8502-F88E2C965073', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2026-07-03', 8, N'Mild', N'الأعراض ضمن المستوى الخفيف، استجابة جيدة للتدخلات العلاجية.', N'Active', '2026-07-03 12:00:00', '2026-07-03 12:00:00'),
('BBD0B5C0-0F84-44D4-81D7-65C19F8F73AA', '2557FC9B-7599-4EB3-9FA4-7DCD1EAE6EDA', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2026-03-27', 17, N'Severe', N'تسجل الدرجات مستوى مرتفعاً من الأعراض، يوصى بمتابعة عن قرب وتكثيف الجلسات.', N'Reviewed', '2026-03-27 12:00:00', '2026-03-27 12:00:00'),
('F7BA474C-829B-494B-95FE-1203D42351A8', '2557FC9B-7599-4EB3-9FA4-7DCD1EAE6EDA', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2026-04-30', 17, N'Severe', N'تسجل الدرجات مستوى مرتفعاً من الأعراض، يوصى بمتابعة عن قرب وتكثيف الجلسات.', N'Reviewed', '2026-04-30 12:00:00', '2026-04-30 12:00:00'),
('32065EB8-9844-42E6-BE2A-A105B7ACA386', '2557FC9B-7599-4EB3-9FA4-7DCD1EAE6EDA', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2026-06-02', 14, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Active', '2026-06-02 12:00:00', '2026-06-02 12:00:00'),
('F51F9A33-13E0-4B4C-8F97-0137C6286B3C', '87C46097-7D04-4AD1-89C2-13967E95301F', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2026-04-17', 18, N'Severe', N'تسجل الدرجات مستوى مرتفعاً من الأعراض، يوصى بمتابعة عن قرب وتكثيف الجلسات.', N'Reviewed', '2026-04-17 12:00:00', '2026-04-17 12:00:00'),
('D95811DE-7B25-4290-B3DA-CBA8A5F58E93', '87C46097-7D04-4AD1-89C2-13967E95301F', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2026-05-07', 16, N'Severe', N'تسجل الدرجات مستوى مرتفعاً من الأعراض، يوصى بمتابعة عن قرب وتكثيف الجلسات.', N'Reviewed', '2026-05-07 12:00:00', '2026-05-07 12:00:00'),
('9C0DC1FC-158C-46B4-BCE2-45BD3076D3EF', '87C46097-7D04-4AD1-89C2-13967E95301F', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2026-05-27', 17, N'Severe', N'تسجل الدرجات مستوى مرتفعاً من الأعراض، يوصى بمتابعة عن قرب وتكثيف الجلسات.', N'Reviewed', '2026-05-27 12:00:00', '2026-05-27 12:00:00'),
('2E430BA0-D2FC-4767-91C4-7C2B14AFFCD0', '87C46097-7D04-4AD1-89C2-13967E95301F', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2026-06-16', 12, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Active', '2026-06-16 12:00:00', '2026-06-16 12:00:00'),
('3AFFF271-64A6-4177-B068-765843258DF7', '0B875E9D-0EE5-40EF-9CC6-451259EEADB3', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2026-02-19', 12, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Reviewed', '2026-02-19 12:00:00', '2026-02-19 12:00:00'),
('CE26271B-A22C-4142-99B0-5D9A4B3BD3AF', '0B875E9D-0EE5-40EF-9CC6-451259EEADB3', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2026-03-25', 11, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Reviewed', '2026-03-25 12:00:00', '2026-03-25 12:00:00'),
('D18CA96E-7E8D-4A46-A65C-54DFA83A682C', '0B875E9D-0EE5-40EF-9CC6-451259EEADB3', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2026-04-29', 12, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Reviewed', '2026-04-29 12:00:00', '2026-04-29 12:00:00'),
('69CD5E54-D4FD-46A6-9C76-7A661CA82513', '0B875E9D-0EE5-40EF-9CC6-451259EEADB3', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2026-06-02', 9, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Active', '2026-06-02 12:00:00', '2026-06-02 12:00:00'),
('13FD135A-8C02-48D4-B6CA-019D02312ADE', 'ED4A6B23-9515-4C85-9110-1C4CC21FAA9E', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2026-02-14', 18, N'Severe', N'تسجل الدرجات مستوى مرتفعاً من الأعراض، يوصى بمتابعة عن قرب وتكثيف الجلسات.', N'Reviewed', '2026-02-14 12:00:00', '2026-02-14 12:00:00'),
('3072F020-BE3D-4D6F-948F-6DA7D47A274F', 'ED4A6B23-9515-4C85-9110-1C4CC21FAA9E', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2026-03-22', 17, N'Severe', N'تسجل الدرجات مستوى مرتفعاً من الأعراض، يوصى بمتابعة عن قرب وتكثيف الجلسات.', N'Reviewed', '2026-03-22 12:00:00', '2026-03-22 12:00:00'),
('380A8A8A-1C1B-44D2-BC83-D301AFF1F2F1', 'ED4A6B23-9515-4C85-9110-1C4CC21FAA9E', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2026-04-26', 15, N'Severe', N'تسجل الدرجات مستوى مرتفعاً من الأعراض، يوصى بمتابعة عن قرب وتكثيف الجلسات.', N'Reviewed', '2026-04-26 12:00:00', '2026-04-26 12:00:00'),
('37371170-8F99-4AD4-9BF3-FC022008D980', 'ED4A6B23-9515-4C85-9110-1C4CC21FAA9E', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2026-06-01', 12, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Active', '2026-06-01 12:00:00', '2026-06-01 12:00:00'),
('E3F6EED7-4A37-4443-A35A-4B9F88524183', '69F104EE-9B40-40CF-A029-EC2A532693FA', NULL, 'BA53959A-A392-4E40-A220-6B0FEFF1F113', N'DASS-21', '2026-04-26', 55, N'Severe', N'تسجل الدرجات مستوى مرتفعاً من الأعراض، يوصى بمتابعة عن قرب وتكثيف الجلسات.', N'Reviewed', '2026-04-26 12:00:00', '2026-04-26 12:00:00'),
('C5822853-5E85-441A-9941-1D83FB910B9C', '69F104EE-9B40-40CF-A029-EC2A532693FA', NULL, 'BA53959A-A392-4E40-A220-6B0FEFF1F113', N'DASS-21', '2026-05-14', 48, N'Severe', N'تسجل الدرجات مستوى مرتفعاً من الأعراض، يوصى بمتابعة عن قرب وتكثيف الجلسات.', N'Reviewed', '2026-05-14 12:00:00', '2026-05-14 12:00:00'),
('9E6A4083-BBBF-4DC5-9B56-9C89D2729460', '69F104EE-9B40-40CF-A029-EC2A532693FA', NULL, 'BA53959A-A392-4E40-A220-6B0FEFF1F113', N'DASS-21', '2026-05-31', 41, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Reviewed', '2026-05-31 12:00:00', '2026-05-31 12:00:00'),
('28E2DFEE-8086-46C6-B94A-1FC3EC2F0CB3', '69F104EE-9B40-40CF-A029-EC2A532693FA', NULL, 'BA53959A-A392-4E40-A220-6B0FEFF1F113', N'DASS-21', '2026-06-18', 37, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Active', '2026-06-18 12:00:00', '2026-06-18 12:00:00'),
('4DCD43A7-8E4E-4AA1-94A0-D371A2726698', '56A8CAE5-A2FA-4149-B2D0-3E5B35F37D3B', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2026-05-12', 12, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Reviewed', '2026-05-12 12:00:00', '2026-05-12 12:00:00'),
('DD7EE205-4972-4FDC-952F-5BB6B4588229', '56A8CAE5-A2FA-4149-B2D0-3E5B35F37D3B', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2026-05-26', 11, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Reviewed', '2026-05-26 12:00:00', '2026-05-26 12:00:00'),
('6D6E978C-CD73-43D6-A6A2-138311B374F1', '56A8CAE5-A2FA-4149-B2D0-3E5B35F37D3B', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2026-06-08', 11, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Reviewed', '2026-06-08 12:00:00', '2026-06-08 12:00:00'),
('F27D69A8-3101-42B9-920E-5921DBDE0C44', '56A8CAE5-A2FA-4149-B2D0-3E5B35F37D3B', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2026-06-22', 10, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Active', '2026-06-22 12:00:00', '2026-06-22 12:00:00'),
('678C8D63-4BBE-4241-9EC5-79A6996BC9B8', '85B46DDD-B1A6-4148-BB18-D7DB4172EAF2', NULL, 'BA53959A-A392-4E40-A220-6B0FEFF1F113', N'DASS-21', '2025-11-08', 56, N'Severe', N'تسجل الدرجات مستوى مرتفعاً من الأعراض، يوصى بمتابعة عن قرب وتكثيف الجلسات.', N'Reviewed', '2025-11-08 12:00:00', '2025-11-08 12:00:00'),
('EA7B1C1A-47AA-46F9-BF84-A48053978DE3', '85B46DDD-B1A6-4148-BB18-D7DB4172EAF2', NULL, 'BA53959A-A392-4E40-A220-6B0FEFF1F113', N'DASS-21', '2026-01-07', 49, N'Severe', N'تسجل الدرجات مستوى مرتفعاً من الأعراض، يوصى بمتابعة عن قرب وتكثيف الجلسات.', N'Reviewed', '2026-01-07 12:00:00', '2026-01-07 12:00:00'),
('8E11E239-8398-4507-A9CE-445E02F2DD9E', '85B46DDD-B1A6-4148-BB18-D7DB4172EAF2', NULL, 'BA53959A-A392-4E40-A220-6B0FEFF1F113', N'DASS-21', '2026-03-08', 42, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Reviewed', '2026-03-08 12:00:00', '2026-03-08 12:00:00'),
('E04ACBEE-85CA-46AB-B116-6795884AE587', '85B46DDD-B1A6-4148-BB18-D7DB4172EAF2', NULL, 'BA53959A-A392-4E40-A220-6B0FEFF1F113', N'DASS-21', '2026-05-07', 36, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Active', '2026-05-07 12:00:00', '2026-05-07 12:00:00'),
('942A87B3-15BB-4EE4-9C3A-85CDC4530517', 'D5AF7A7D-26C9-4CD6-854B-11C4F6A65218', NULL, 'BA53959A-A392-4E40-A220-6B0FEFF1F113', N'DASS-21', '2025-12-09', 56, N'Severe', N'تسجل الدرجات مستوى مرتفعاً من الأعراض، يوصى بمتابعة عن قرب وتكثيف الجلسات.', N'Reviewed', '2025-12-09 12:00:00', '2025-12-09 12:00:00'),
('57605129-0FDA-4D45-AA84-107C9980A2A4', 'D5AF7A7D-26C9-4CD6-854B-11C4F6A65218', NULL, 'BA53959A-A392-4E40-A220-6B0FEFF1F113', N'DASS-21', '2026-02-17', 48, N'Severe', N'تسجل الدرجات مستوى مرتفعاً من الأعراض، يوصى بمتابعة عن قرب وتكثيف الجلسات.', N'Reviewed', '2026-02-17 12:00:00', '2026-02-17 12:00:00'),
('3696F190-0B93-4A05-8DDE-BD75BF990E25', 'D5AF7A7D-26C9-4CD6-854B-11C4F6A65218', NULL, 'BA53959A-A392-4E40-A220-6B0FEFF1F113', N'DASS-21', '2026-04-27', 38, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Active', '2026-04-27 12:00:00', '2026-04-27 12:00:00'),
('4F7689DA-FECA-461D-B8CB-193560E25620', 'AEB38627-5539-4776-8AEC-69F91DD159F1', NULL, '36B3C4A8-51E1-4BEC-B4A9-746B83073EE9', N'PHQ-9', '2025-12-30', 18, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Reviewed', '2025-12-30 12:00:00', '2025-12-30 12:00:00'),
('F39F0759-97B3-43FE-8F9F-105772C5DDDC', 'AEB38627-5539-4776-8AEC-69F91DD159F1', NULL, '36B3C4A8-51E1-4BEC-B4A9-746B83073EE9', N'PHQ-9', '2026-02-15', 12, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Reviewed', '2026-02-15 12:00:00', '2026-02-15 12:00:00'),
('268C0D5A-CB84-48F0-BF7F-870BE04766EB', 'AEB38627-5539-4776-8AEC-69F91DD159F1', NULL, '36B3C4A8-51E1-4BEC-B4A9-746B83073EE9', N'PHQ-9', '2026-04-03', 14, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Reviewed', '2026-04-03 12:00:00', '2026-04-03 12:00:00'),
('554D84A4-E5D9-4CF0-8A7D-933DF42B9C28', 'AEB38627-5539-4776-8AEC-69F91DD159F1', NULL, '36B3C4A8-51E1-4BEC-B4A9-746B83073EE9', N'PHQ-9', '2026-05-20', 10, N'Mild', N'الأعراض ضمن المستوى الخفيف، استجابة جيدة للتدخلات العلاجية.', N'Active', '2026-05-20 12:00:00', '2026-05-20 12:00:00'),
('4340A0A0-0D9D-4B63-B605-0FB1475ABAFE', '00CC925F-9671-4970-ADC6-5C26F8FBF8E5', NULL, 'BA53959A-A392-4E40-A220-6B0FEFF1F113', N'DASS-21', '2026-05-03', 42, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Reviewed', '2026-05-03 12:00:00', '2026-05-03 12:00:00'),
('80D0AED8-6573-4BC2-9F6E-03BC9E065F39', '00CC925F-9671-4970-ADC6-5C26F8FBF8E5', NULL, 'BA53959A-A392-4E40-A220-6B0FEFF1F113', N'DASS-21', '2026-06-04', 29, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Active', '2026-06-04 12:00:00', '2026-06-04 12:00:00'),
('D848D1AA-AEF1-41BC-9182-41116F0A3C0E', '5947B1E9-EACB-4CD1-A014-69DBDA364362', NULL, 'BA53959A-A392-4E40-A220-6B0FEFF1F113', N'DASS-21', '2026-02-15', 53, N'Severe', N'تسجل الدرجات مستوى مرتفعاً من الأعراض، يوصى بمتابعة عن قرب وتكثيف الجلسات.', N'Reviewed', '2026-02-15 12:00:00', '2026-02-15 12:00:00'),
('41CA47D5-065D-4574-BEFA-F49A8A03FB61', '5947B1E9-EACB-4CD1-A014-69DBDA364362', NULL, 'BA53959A-A392-4E40-A220-6B0FEFF1F113', N'DASS-21', '2026-04-27', 34, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Active', '2026-04-27 12:00:00', '2026-04-27 12:00:00'),
('AB717E9A-88C6-4FCB-834B-9EB004212433', 'D05B32B7-0D94-4041-91CE-8952FF6C525A', NULL, 'BA53959A-A392-4E40-A220-6B0FEFF1F113', N'DASS-21', '2025-11-12', 45, N'Severe', N'تسجل الدرجات مستوى مرتفعاً من الأعراض، يوصى بمتابعة عن قرب وتكثيف الجلسات.', N'Reviewed', '2025-11-12 12:00:00', '2025-11-12 12:00:00'),
('DBC3738C-A986-483B-BC9A-F8B2C3079086', 'D05B32B7-0D94-4041-91CE-8952FF6C525A', NULL, 'BA53959A-A392-4E40-A220-6B0FEFF1F113', N'DASS-21', '2026-03-10', 31, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Active', '2026-03-10 12:00:00', '2026-03-10 12:00:00'),
('BF254371-FBFA-4E50-A542-C35CB7E0F24E', '7F9C808B-66FB-44EC-AE2F-D987CCB0D98A', NULL, 'BA53959A-A392-4E40-A220-6B0FEFF1F113', N'DASS-21', '2026-03-25', 49, N'Severe', N'تسجل الدرجات مستوى مرتفعاً من الأعراض، يوصى بمتابعة عن قرب وتكثيف الجلسات.', N'Reviewed', '2026-03-25 12:00:00', '2026-03-25 12:00:00'),
('C4BB47F2-D2CD-4D91-BCCC-EA940BB64AE9', '7F9C808B-66FB-44EC-AE2F-D987CCB0D98A', NULL, 'BA53959A-A392-4E40-A220-6B0FEFF1F113', N'DASS-21', '2026-04-29', 40, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Reviewed', '2026-04-29 12:00:00', '2026-04-29 12:00:00'),
('2834B217-D4A7-4EC1-A3BC-A742992CD5B5', '7F9C808B-66FB-44EC-AE2F-D987CCB0D98A', NULL, 'BA53959A-A392-4E40-A220-6B0FEFF1F113', N'DASS-21', '2026-06-02', 33, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Active', '2026-06-02 12:00:00', '2026-06-02 12:00:00'),
('BCA28E5F-C833-4C89-97CD-95282F89207E', '080C5383-4556-405D-BBEC-38C843811EBD', NULL, '36B3C4A8-51E1-4BEC-B4A9-746B83073EE9', N'PHQ-9', '2026-07-03', 19, N'Severe', N'تسجل الدرجات مستوى مرتفعاً من الأعراض، يوصى بمتابعة عن قرب وتكثيف الجلسات.', N'Reviewed', '2026-07-03 12:00:00', '2026-07-03 12:00:00'),
('B1EB8C5C-CBD4-4B19-95F1-F325F96CD74F', '080C5383-4556-405D-BBEC-38C843811EBD', NULL, '36B3C4A8-51E1-4BEC-B4A9-746B83073EE9', N'PHQ-9', '2026-07-09', 19, N'Severe', N'تسجل الدرجات مستوى مرتفعاً من الأعراض، يوصى بمتابعة عن قرب وتكثيف الجلسات.', N'Reviewed', '2026-07-09 12:00:00', '2026-07-09 12:00:00'),
('22B2D813-308E-4373-8A8F-74BF207B5B20', '080C5383-4556-405D-BBEC-38C843811EBD', NULL, '36B3C4A8-51E1-4BEC-B4A9-746B83073EE9', N'PHQ-9', '2026-07-14', 15, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Active', '2026-07-14 12:00:00', '2026-07-14 12:00:00'),
('A948F55C-B5E4-4E81-B509-3477DC2CCA5D', '5A0DA59E-4AE0-4CF6-B7CD-5BDB2F91D5E7', NULL, 'BA53959A-A392-4E40-A220-6B0FEFF1F113', N'DASS-21', '2026-02-27', 48, N'Severe', N'تسجل الدرجات مستوى مرتفعاً من الأعراض، يوصى بمتابعة عن قرب وتكثيف الجلسات.', N'Reviewed', '2026-02-27 12:00:00', '2026-02-27 12:00:00'),
('EFE20376-FD6C-4EBB-8A96-224C4E80DAEF', '5A0DA59E-4AE0-4CF6-B7CD-5BDB2F91D5E7', NULL, 'BA53959A-A392-4E40-A220-6B0FEFF1F113', N'DASS-21', '2026-03-31', 42, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Reviewed', '2026-03-31 12:00:00', '2026-03-31 12:00:00'),
('B8A825C6-CA02-48E1-8734-A501F811958C', '5A0DA59E-4AE0-4CF6-B7CD-5BDB2F91D5E7', NULL, 'BA53959A-A392-4E40-A220-6B0FEFF1F113', N'DASS-21', '2026-05-03', 39, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Reviewed', '2026-05-03 12:00:00', '2026-05-03 12:00:00'),
('4E76ABD1-2EE3-477D-A355-2336E8B535AE', '5A0DA59E-4AE0-4CF6-B7CD-5BDB2F91D5E7', NULL, 'BA53959A-A392-4E40-A220-6B0FEFF1F113', N'DASS-21', '2026-06-04', 30, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Active', '2026-06-04 12:00:00', '2026-06-04 12:00:00'),
('0FC36253-5908-412F-9C2D-926714EAF0D2', '0004E60D-C638-4639-BCFA-0AD58724C84E', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2026-06-24', 13, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Reviewed', '2026-06-24 12:00:00', '2026-06-24 12:00:00'),
('0EEEDE31-2545-44A9-8C18-33772FB59C64', '0004E60D-C638-4639-BCFA-0AD58724C84E', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2026-06-28', 11, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Reviewed', '2026-06-28 12:00:00', '2026-06-28 12:00:00'),
('4ED5ADBB-2CB9-4042-AFF4-2BA3B27C54D6', '0004E60D-C638-4639-BCFA-0AD58724C84E', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2026-07-03', 10, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Reviewed', '2026-07-03 12:00:00', '2026-07-03 12:00:00'),
('2B604597-AE03-4A4E-8EEF-8DF474CCFEBE', '0004E60D-C638-4639-BCFA-0AD58724C84E', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2026-07-07', 9, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Active', '2026-07-07 12:00:00', '2026-07-07 12:00:00'),
('35B55A67-C306-4DFB-A6E5-9E694CBA4055', '81B640AC-DBBD-4F29-831E-F36B5C6A7580', NULL, '36B3C4A8-51E1-4BEC-B4A9-746B83073EE9', N'PHQ-9', '2026-04-06', 21, N'Severe', N'تسجل الدرجات مستوى مرتفعاً من الأعراض، يوصى بمتابعة عن قرب وتكثيف الجلسات.', N'Reviewed', '2026-04-06 12:00:00', '2026-04-06 12:00:00'),
('A010FDC7-D150-4742-A012-D89F3F997819', '81B640AC-DBBD-4F29-831E-F36B5C6A7580', NULL, '36B3C4A8-51E1-4BEC-B4A9-746B83073EE9', N'PHQ-9', '2026-05-07', 16, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Reviewed', '2026-05-07 12:00:00', '2026-05-07 12:00:00'),
('8F6D7989-21C4-4EFD-B4BC-A5A89938D0F6', '81B640AC-DBBD-4F29-831E-F36B5C6A7580', NULL, '36B3C4A8-51E1-4BEC-B4A9-746B83073EE9', N'PHQ-9', '2026-06-06', 16, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Active', '2026-06-06 12:00:00', '2026-06-06 12:00:00'),
('F03C4C4A-B142-408A-A7AC-6142A01CA576', '85C8A7BC-BBE9-4D03-B8D2-89AFEE304102', NULL, 'BA53959A-A392-4E40-A220-6B0FEFF1F113', N'DASS-21', '2026-03-10', 36, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Reviewed', '2026-03-10 12:00:00', '2026-03-10 12:00:00'),
('F2AA4EFB-5ABA-44DC-8F23-20C10391DAEA', '85C8A7BC-BBE9-4D03-B8D2-89AFEE304102', NULL, 'BA53959A-A392-4E40-A220-6B0FEFF1F113', N'DASS-21', '2026-04-08', 31, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Reviewed', '2026-04-08 12:00:00', '2026-04-08 12:00:00'),
('4A75ACA7-20C8-4649-8406-74A8AF28CB0A', '85C8A7BC-BBE9-4D03-B8D2-89AFEE304102', NULL, 'BA53959A-A392-4E40-A220-6B0FEFF1F113', N'DASS-21', '2026-05-08', 29, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Reviewed', '2026-05-08 12:00:00', '2026-05-08 12:00:00'),
('06D01B99-7904-4DB2-B25C-21B0300C2FE7', '85C8A7BC-BBE9-4D03-B8D2-89AFEE304102', NULL, 'BA53959A-A392-4E40-A220-6B0FEFF1F113', N'DASS-21', '2026-06-06', 21, N'Mild', N'الأعراض ضمن المستوى الخفيف، استجابة جيدة للتدخلات العلاجية.', N'Active', '2026-06-06 12:00:00', '2026-06-06 12:00:00'),
('A48E43C5-B8EB-4310-9A12-9054A0268997', '46ACC094-18A3-4CCF-80C4-95D298EF4031', NULL, 'BA53959A-A392-4E40-A220-6B0FEFF1F113', N'DASS-21', '2026-03-19', 42, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Reviewed', '2026-03-19 12:00:00', '2026-03-19 12:00:00'),
('D45712C3-4E6D-43BF-9E06-A596452079DA', '46ACC094-18A3-4CCF-80C4-95D298EF4031', NULL, 'BA53959A-A392-4E40-A220-6B0FEFF1F113', N'DASS-21', '2026-04-15', 39, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Reviewed', '2026-04-15 12:00:00', '2026-04-15 12:00:00'),
('B26C1600-2D75-48B1-B85B-83913A99A115', '46ACC094-18A3-4CCF-80C4-95D298EF4031', NULL, 'BA53959A-A392-4E40-A220-6B0FEFF1F113', N'DASS-21', '2026-05-13', 32, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Reviewed', '2026-05-13 12:00:00', '2026-05-13 12:00:00'),
('B545C9DD-E7F9-4CB3-9994-8D442A107915', '46ACC094-18A3-4CCF-80C4-95D298EF4031', NULL, 'BA53959A-A392-4E40-A220-6B0FEFF1F113', N'DASS-21', '2026-06-09', 29, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Active', '2026-06-09 12:00:00', '2026-06-09 12:00:00'),
('DEE25C7B-B237-4C3F-85E7-49597C030B40', 'F300A791-B73E-4A86-90EA-4D1EE67BBE48', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2026-06-11', 18, N'Severe', N'تسجل الدرجات مستوى مرتفعاً من الأعراض، يوصى بمتابعة عن قرب وتكثيف الجلسات.', N'Reviewed', '2026-06-11 12:00:00', '2026-06-11 12:00:00'),
('E9EA5AE0-10BD-4A75-A1DD-07264326E3B3', 'F300A791-B73E-4A86-90EA-4D1EE67BBE48', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2026-06-19', 12, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Reviewed', '2026-06-19 12:00:00', '2026-06-19 12:00:00'),
('C4DA7CB5-5240-4851-8958-1506742790E1', 'F300A791-B73E-4A86-90EA-4D1EE67BBE48', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2026-06-28', 10, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Active', '2026-06-28 12:00:00', '2026-06-28 12:00:00'),
('FD5B3DE7-AA02-4B48-96C7-F4CD0DCBF6C9', '74FDAC62-9C1A-472B-AA93-BCDC4103514D', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2026-03-17', 14, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Reviewed', '2026-03-17 12:00:00', '2026-03-17 12:00:00'),
('B7A81B9F-0791-4B11-A87B-9E8EA3E39E7D', '74FDAC62-9C1A-472B-AA93-BCDC4103514D', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2026-05-12', 10, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Active', '2026-05-12 12:00:00', '2026-05-12 12:00:00'),
('57DC1E0F-0E87-4E45-B3D3-09C6F972C65B', '5A2EBBD8-21B9-47E7-881C-B09F16F70FE6', NULL, '36B3C4A8-51E1-4BEC-B4A9-746B83073EE9', N'PHQ-9', '2026-06-09', 23, N'Severe', N'تسجل الدرجات مستوى مرتفعاً من الأعراض، يوصى بمتابعة عن قرب وتكثيف الجلسات.', N'Reviewed', '2026-06-09 12:00:00', '2026-06-09 12:00:00'),
('D19CC1F0-8702-4CF5-9E84-CE9E36E5086B', '5A2EBBD8-21B9-47E7-881C-B09F16F70FE6', NULL, '36B3C4A8-51E1-4BEC-B4A9-746B83073EE9', N'PHQ-9', '2026-06-16', 18, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Reviewed', '2026-06-16 12:00:00', '2026-06-16 12:00:00'),
('150E3F99-436B-47BC-B82E-C2BD7DCFB1F7', '5A2EBBD8-21B9-47E7-881C-B09F16F70FE6', NULL, '36B3C4A8-51E1-4BEC-B4A9-746B83073EE9', N'PHQ-9', '2026-06-22', 18, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Reviewed', '2026-06-22 12:00:00', '2026-06-22 12:00:00'),
('10DC5A03-934F-46DF-BD17-1BBB223ADD77', '5A2EBBD8-21B9-47E7-881C-B09F16F70FE6', NULL, '36B3C4A8-51E1-4BEC-B4A9-746B83073EE9', N'PHQ-9', '2026-06-29', 16, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Active', '2026-06-29 12:00:00', '2026-06-29 12:00:00'),
('CE580938-8AA9-432B-9094-B01D29D226D1', 'CC37E596-7078-4E46-938E-D6B59CA67B02', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2026-04-10', 15, N'Severe', N'تسجل الدرجات مستوى مرتفعاً من الأعراض، يوصى بمتابعة عن قرب وتكثيف الجلسات.', N'Reviewed', '2026-04-10 12:00:00', '2026-04-10 12:00:00'),
('9E691C49-7FD6-490B-96B0-B23C7D3845D5', 'CC37E596-7078-4E46-938E-D6B59CA67B02', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2026-05-09', 12, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Reviewed', '2026-05-09 12:00:00', '2026-05-09 12:00:00'),
('87A9CFAE-B744-4B69-8F60-D88EDFC8AE8B', 'CC37E596-7078-4E46-938E-D6B59CA67B02', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2026-06-07', 11, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Active', '2026-06-07 12:00:00', '2026-06-07 12:00:00'),
('A9D8AC11-92B3-439F-A01F-FAA396B474F5', '5F6B076C-526F-4F65-B5E3-F416520D1841', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2026-04-06', 15, N'Severe', N'تسجل الدرجات مستوى مرتفعاً من الأعراض، يوصى بمتابعة عن قرب وتكثيف الجلسات.', N'Reviewed', '2026-04-06 12:00:00', '2026-04-06 12:00:00'),
('E4A03797-0A95-4FF1-BB81-50C868862C65', '5F6B076C-526F-4F65-B5E3-F416520D1841', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2026-05-07', 15, N'Severe', N'تسجل الدرجات مستوى مرتفعاً من الأعراض، يوصى بمتابعة عن قرب وتكثيف الجلسات.', N'Reviewed', '2026-05-07 12:00:00', '2026-05-07 12:00:00'),
('419CDF1D-CD61-4A7B-B34C-FAFA37627FC0', '5F6B076C-526F-4F65-B5E3-F416520D1841', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2026-06-06', 10, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Active', '2026-06-06 12:00:00', '2026-06-06 12:00:00'),
('3CECF2FA-B527-48BB-850B-F00F6F612E21', '8BDD5425-225E-4657-AF47-CA87A9625781', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2026-02-03', 17, N'Severe', N'تسجل الدرجات مستوى مرتفعاً من الأعراض، يوصى بمتابعة عن قرب وتكثيف الجلسات.', N'Reviewed', '2026-02-03 12:00:00', '2026-02-03 12:00:00'),
('8E75A709-0D84-4917-B90F-EE025876D767', '8BDD5425-225E-4657-AF47-CA87A9625781', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2026-04-20', 13, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Active', '2026-04-20 12:00:00', '2026-04-20 12:00:00'),
('954CC696-503C-4FDC-9580-C7C73BE5D9AD', 'D149A229-6389-48F8-91B0-BE1D2E2900B1', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2025-12-14', 13, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Reviewed', '2025-12-14 12:00:00', '2025-12-14 12:00:00'),
('160E4629-F25B-46AC-8DA6-2478A69FE44C', 'D149A229-6389-48F8-91B0-BE1D2E2900B1', NULL, 'CF229ED4-A0D2-4D74-8C49-B5E5C12909BB', N'GAD-7', '2026-03-26', 11, N'Moderate', N'تحسن طفيف ملحوظ مقارنة بالتقييم السابق، الاستمرار في خطة العلاج الحالية.', N'Active', '2026-03-26 12:00:00', '2026-03-26 12:00:00');

-- ===== Exercises =====
INSERT INTO [Exercises] ([Id], [PatientId], [Description], [Frequency], [StartDate], [DueDate], [DurationMinutes], [Difficulty], [Status], [CreatedAt], [UpdatedAt])
VALUES
('907989D1-B3E1-4866-9C42-F690CEC2EDD1', '692BFE83-C7BF-4FAA-9245-0EEDE315EBDB', N'تمرين: استراحة الرأفة الذاتية (Self-Compassion Break) - تمرين قصير للتعامل مع الذات بلطف في أوقات الضغط.', N'Weekly', '2026-07-08', '2026-08-27', 10, N'Easy', N'Active', '2026-07-08 12:00:00', '2026-07-08 12:00:00'),
('4BA6D26C-63EB-4427-893A-DE6F3249BEB1', '692BFE83-C7BF-4FAA-9245-0EEDE315EBDB', N'تمرين: تقنية التأريض 5-4-3-2-1 (Grounding Technique) - تمرين حسي لإعادة التركيز إلى اللحظة الحالية.', N'Biweekly', '2026-07-13', '2026-08-13', 15, N'Easy', N'Active', '2026-07-13 12:00:00', '2026-07-13 12:00:00'),
('59470738-54A0-498F-AFCD-AE72B71EEEFA', '692BFE83-C7BF-4FAA-9245-0EEDE315EBDB', N'تمرين: يوميات الامتنان (Gratitude Journal) - تدوين ثلاثة أشياء إيجابية حدثت خلال اليوم.', N'Weekly', '2026-07-11', '2026-08-30', 10, N'Easy', N'Active', '2026-07-11 12:00:00', '2026-07-11 12:00:00'),
('E13B4964-28FD-4FA1-89A8-D29488FBEB82', '5912AA70-D127-4B2A-95B6-62222A464244', N'تمرين: مسح الجسد للاسترخاء (Body Scan Relaxation) - تمرين وعي جسدي تدريجي من القدمين إلى الرأس.', N'Daily', '2026-05-26', '2026-07-01', 15, N'Medium', N'Completed', '2026-05-26 12:00:00', '2026-05-26 12:00:00'),
('8F00AD90-507D-4558-83AE-9BEA3253936E', '5912AA70-D127-4B2A-95B6-62222A464244', N'تمرين: التنفس العميق (Deep Breathing) - تمرين تنفس بطيء وعميق لتهدئة الجهاز العصبي.', N'Biweekly', '2026-05-23', '2026-07-20', 10, N'Easy', N'Active', '2026-05-23 12:00:00', '2026-05-23 12:00:00'),
('51398AA0-9388-4596-AC24-594A512CC0B5', 'D091A7F4-A73A-4CCE-920E-0AB7D4C36536', N'تمرين: استراحة الرأفة الذاتية (Self-Compassion Break) - تمرين قصير للتعامل مع الذات بلطف في أوقات الضغط.', N'Daily', '2025-07-03', '2025-08-21', 10, N'Easy', N'Abandoned', '2025-07-03 12:00:00', '2025-07-03 12:00:00'),
('9E6CFF62-FA31-4620-962B-F57F9BE77344', 'D091A7F4-A73A-4CCE-920E-0AB7D4C36536', N'تمرين: الحديث الذاتي الإيجابي (Positive Self Talk) - استبدال الأفكار السلبية بعبارات داعمة للذات.', N'Weekly', '2025-07-07', '2025-08-23', 10, N'Medium', N'Completed', '2025-07-07 12:00:00', '2025-07-07 12:00:00'),
('776FDA27-E3CA-42DE-9CF4-7850A7F38DDE', 'D091A7F4-A73A-4CCE-920E-0AB7D4C36536', N'تمرين: تتبع المزاج اليومي (Mood Tracking) - تسجيل الحالة المزاجية ثلاث مرات يومياً.', N'Biweekly', '2025-07-05', '2025-08-24', 5, N'Easy', N'Abandoned', '2025-07-05 12:00:00', '2025-07-05 12:00:00'),
('6F63E5BD-FEAE-4C11-9A77-37E1E9260F06', '3A5350AB-011B-41CA-A9F0-A7B2289AE70C', N'تمرين: الحديث الذاتي الإيجابي (Positive Self Talk) - استبدال الأفكار السلبية بعبارات داعمة للذات.', N'Daily', '2025-12-04', '2026-02-19', 10, N'Medium', N'Completed', '2025-12-04 12:00:00', '2025-12-04 12:00:00'),
('4AE87BEB-C39B-488F-9943-C5C71640BEB2', '3A5350AB-011B-41CA-A9F0-A7B2289AE70C', N'تمرين: الاسترخاء العضلي التدريجي (Progressive Muscle Relaxation) - شد وإرخاء مجموعات العضلات تباعاً لتقليل التوتر الجسدي.', N'Daily', '2025-11-29', '2026-02-11', 20, N'Medium', N'Completed', '2025-11-29 12:00:00', '2025-11-29 12:00:00'),
('7BA7486A-A9E0-444A-9EBC-556A4ACFE378', '3A5350AB-011B-41CA-A9F0-A7B2289AE70C', N'تمرين: سلم التعرض الاجتماعي (Social Exposure Ladder) - التعرض التدريجي لمواقف اجتماعية مثيرة للقلق.', N'Daily', '2025-12-06', '2026-02-12', 30, N'Hard', N'Completed', '2025-12-06 12:00:00', '2025-12-06 12:00:00'),
('75927909-0E87-4F6B-99A2-4CC9C9E27D6A', '3A5350AB-011B-41CA-A9F0-A7B2289AE70C', N'تمرين: سجل الأفكار (Thought Record - CBT) - تحديد الأفكار التلقائية السلبية وإعادة صياغتها منطقياً.', N'Daily', '2025-11-30', '2025-12-28', 25, N'Hard', N'Completed', '2025-11-30 12:00:00', '2025-11-30 12:00:00'),
('F0BB3BDF-92E5-400C-AEF7-EF4612509409', 'E07477FB-C7F6-4846-A2AB-0ED1F6DCB7AC', N'تمرين: تتبع المزاج اليومي (Mood Tracking) - تسجيل الحالة المزاجية ثلاث مرات يومياً.', N'Weekly', '2025-09-26', '2025-11-26', 5, N'Easy', N'Completed', '2025-09-26 12:00:00', '2025-09-26 12:00:00'),
('847E694C-092C-40CA-BC58-C531586F1F5E', 'E07477FB-C7F6-4846-A2AB-0ED1F6DCB7AC', N'تمرين: تمرين وقت القلق المحدد (Worry Time Exercise) - تخصيص 15 دقيقة يومياً فقط للتفكير في المخاوف.', N'Weekly', '2025-09-30', '2025-11-12', 15, N'Medium', N'Completed', '2025-09-30 12:00:00', '2025-09-30 12:00:00'),
('B152A7BF-EE40-42DE-BE98-9F9845D9035E', 'E07477FB-C7F6-4846-A2AB-0ED1F6DCB7AC', N'تمرين: مسح الجسد للاسترخاء (Body Scan Relaxation) - تمرين وعي جسدي تدريجي من القدمين إلى الرأس.', N'Weekly', '2025-10-03', '2025-11-02', 15, N'Medium', N'Completed', '2025-10-03 12:00:00', '2025-10-03 12:00:00'),
('E853FBC7-D555-4D8D-B854-4D482EA2EB80', 'E07477FB-C7F6-4846-A2AB-0ED1F6DCB7AC', N'تمرين: الاسترخاء العضلي التدريجي (Progressive Muscle Relaxation) - شد وإرخاء مجموعات العضلات تباعاً لتقليل التوتر الجسدي.', N'Biweekly', '2025-09-27', '2025-11-15', 20, N'Medium', N'Completed', '2025-09-27 12:00:00', '2025-09-27 12:00:00'),
('5AB551C9-3894-46AA-A875-DBF379A5ADC1', 'DC19C765-09E1-4B24-808B-8EFC97E63589', N'تمرين: يوميات الامتنان (Gratitude Journal) - تدوين ثلاثة أشياء إيجابية حدثت خلال اليوم.', N'Weekly', '2026-03-16', '2026-05-12', 10, N'Easy', N'Completed', '2026-03-16 12:00:00', '2026-03-16 12:00:00'),
('128FA5D6-04D3-40C1-BF02-18B517FFCAB0', 'DC19C765-09E1-4B24-808B-8EFC97E63589', N'تمرين: استراحة الرأفة الذاتية (Self-Compassion Break) - تمرين قصير للتعامل مع الذات بلطف في أوقات الضغط.', N'Biweekly', '2026-03-14', '2026-04-28', 10, N'Easy', N'Completed', '2026-03-14 12:00:00', '2026-03-14 12:00:00'),
('47CE33C0-0FF1-4479-83B0-69C48AC1202F', '8F8DC8EB-DCBE-44D2-B913-73C2CE8E75B3', N'تمرين: الحديث الذاتي الإيجابي (Positive Self Talk) - استبدال الأفكار السلبية بعبارات داعمة للذات.', N'Daily', '2026-01-30', '2026-04-12', 10, N'Medium', N'Completed', '2026-01-30 12:00:00', '2026-01-30 12:00:00'),
('9257536E-95FE-4FBD-BC54-E10FFA2CED62', '8F8DC8EB-DCBE-44D2-B913-73C2CE8E75B3', N'تمرين: التنفس العميق (Deep Breathing) - تمرين تنفس بطيء وعميق لتهدئة الجهاز العصبي.', N'Biweekly', '2026-01-27', '2026-04-01', 10, N'Easy', N'Completed', '2026-01-27 12:00:00', '2026-01-27 12:00:00'),
('A0D7F743-526A-4B8C-841D-22A6BCCF732F', '8F8DC8EB-DCBE-44D2-B913-73C2CE8E75B3', N'تمرين: سجل نوبات الغضب (Anger Log) - تسجيل مواقف الغضب ومحفزاتها واستجابات التعامل معها.', N'Daily', '2026-01-28', '2026-03-20', 10, N'Medium', N'Completed', '2026-01-28 12:00:00', '2026-01-28 12:00:00'),
('2BEA48B4-24A0-4E09-9028-458D1CE3F579', '8F8DC8EB-DCBE-44D2-B913-73C2CE8E75B3', N'تمرين: التأمل الموجه (Guided Meditation) - جلسة تأمل صوتية موجهة لتقليل التوتر.', N'Daily', '2026-01-26', '2026-04-15', 20, N'Medium', N'Completed', '2026-01-26 12:00:00', '2026-01-26 12:00:00'),
('95372A36-329F-4AC4-B8DF-18F96CF24ABF', 'F9DD6265-AFC8-44EC-8985-D0E90571FCA6', N'تمرين: استراحة الرأفة الذاتية (Self-Compassion Break) - تمرين قصير للتعامل مع الذات بلطف في أوقات الضغط.', N'Biweekly', '2026-02-11', '2026-04-21', 10, N'Easy', N'Completed', '2026-02-11 12:00:00', '2026-02-11 12:00:00'),
('46D2D003-9146-49A8-89F1-852D99BE15F0', 'F9DD6265-AFC8-44EC-8985-D0E90571FCA6', N'تمرين: الاسترخاء العضلي التدريجي (Progressive Muscle Relaxation) - شد وإرخاء مجموعات العضلات تباعاً لتقليل التوتر الجسدي.', N'Weekly', '2026-02-05', '2026-03-09', 20, N'Medium', N'Completed', '2026-02-05 12:00:00', '2026-02-05 12:00:00'),
('2B5DEB16-BBAE-4FB1-BF25-F4B2DBC27F75', 'F9DD6265-AFC8-44EC-8985-D0E90571FCA6', N'تمرين: سلم التعرض الاجتماعي (Social Exposure Ladder) - التعرض التدريجي لمواقف اجتماعية مثيرة للقلق.', N'Weekly', '2026-02-09', '2026-03-15', 30, N'Hard', N'Completed', '2026-02-09 12:00:00', '2026-02-09 12:00:00'),
('BC5ECDCD-510E-4F63-A4C8-035A68C1ADFB', 'F9DD6265-AFC8-44EC-8985-D0E90571FCA6', N'تمرين: التنفس العميق (Deep Breathing) - تمرين تنفس بطيء وعميق لتهدئة الجهاز العصبي.', N'Daily', '2026-02-08', '2026-03-10', 10, N'Easy', N'Completed', '2026-02-08 12:00:00', '2026-02-08 12:00:00'),
('901EE028-7107-4753-A19B-888138AB9BE8', 'A8BC8AD9-EC06-4493-8502-F88E2C965073', N'تمرين: سجل الأفكار (Thought Record - CBT) - تحديد الأفكار التلقائية السلبية وإعادة صياغتها منطقياً.', N'Weekly', '2026-06-26', '2026-09-11', 25, N'Hard', N'Active', '2026-06-26 12:00:00', '2026-06-26 12:00:00'),
('A25E0F60-CD12-48FA-A926-586A2E1EBB28', 'A8BC8AD9-EC06-4493-8502-F88E2C965073', N'تمرين: قائمة نظافة النوم (Sleep Hygiene Checklist) - مراجعة عادات ما قبل النوم لتحسين جودة النوم.', N'Weekly', '2026-06-26', '2026-09-14', 10, N'Easy', N'Active', '2026-06-26 12:00:00', '2026-06-26 12:00:00'),
('01986D11-3410-4C5A-8D8C-054C54975AE6', 'A8BC8AD9-EC06-4493-8502-F88E2C965073', N'تمرين: سجل نوبات الغضب (Anger Log) - تسجيل مواقف الغضب ومحفزاتها واستجابات التعامل معها.', N'Weekly', '2026-06-20', '2026-08-17', 10, N'Medium', N'Active', '2026-06-20 12:00:00', '2026-06-20 12:00:00'),
('F353A6EB-0B0E-4826-A303-B3E392F52B0F', 'A8BC8AD9-EC06-4493-8502-F88E2C965073', N'تمرين: الحديث الذاتي الإيجابي (Positive Self Talk) - استبدال الأفكار السلبية بعبارات داعمة للذات.', N'Biweekly', '2026-06-25', '2026-07-23', 10, N'Medium', N'Active', '2026-06-25 12:00:00', '2026-06-25 12:00:00'),
('2785B286-2187-4953-9BCA-C79E211D522E', '2557FC9B-7599-4EB3-9FA4-7DCD1EAE6EDA', N'تمرين: يوميات التأمل في الضغوط (Stress Reflection Journal) - تدوين مصادر الضغط اليومية وطرق التعامل معها.', N'Daily', '2026-02-28', '2026-04-06', 15, N'Medium', N'Completed', '2026-02-28 12:00:00', '2026-02-28 12:00:00'),
('E9E340BC-01DE-4F53-8A24-021AE42BC37D', '2557FC9B-7599-4EB3-9FA4-7DCD1EAE6EDA', N'تمرين: تقنية التأريض 5-4-3-2-1 (Grounding Technique) - تمرين حسي لإعادة التركيز إلى اللحظة الحالية.', N'Daily', '2026-02-25', '2026-04-15', 15, N'Easy', N'Completed', '2026-02-25 12:00:00', '2026-02-25 12:00:00'),
('32111629-8DD8-464B-B653-D738A61738DD', '2557FC9B-7599-4EB3-9FA4-7DCD1EAE6EDA', N'تمرين: سجل الأفكار (Thought Record - CBT) - تحديد الأفكار التلقائية السلبية وإعادة صياغتها منطقياً.', N'Weekly', '2026-03-02', '2026-04-09', 25, N'Hard', N'Abandoned', '2026-03-02 12:00:00', '2026-03-02 12:00:00'),
('7D706C26-4E08-4553-886C-14D328984985', '2557FC9B-7599-4EB3-9FA4-7DCD1EAE6EDA', N'تمرين: مسح الجسد للاسترخاء (Body Scan Relaxation) - تمرين وعي جسدي تدريجي من القدمين إلى الرأس.', N'Weekly', '2026-02-24', '2026-04-06', 15, N'Medium', N'Completed', '2026-02-24 12:00:00', '2026-02-24 12:00:00'),
('2BABD30A-0421-4CDD-8FDB-E17FBB085B72', '87C46097-7D04-4AD1-89C2-13967E95301F', N'تمرين: الحديث الذاتي الإيجابي (Positive Self Talk) - استبدال الأفكار السلبية بعبارات داعمة للذات.', N'Daily', '2026-03-31', '2026-04-30', 10, N'Medium', N'Completed', '2026-03-31 12:00:00', '2026-03-31 12:00:00'),
('7DA8A7AB-0E80-45FC-AE5F-7B3443D8CFB1', '87C46097-7D04-4AD1-89C2-13967E95301F', N'تمرين: تتبع المزاج اليومي (Mood Tracking) - تسجيل الحالة المزاجية ثلاث مرات يومياً.', N'Weekly', '2026-03-31', '2026-06-15', 5, N'Easy', N'Completed', '2026-03-31 12:00:00', '2026-03-31 12:00:00'),
('BC385846-1992-473F-83F8-DC3B2C1F0EC3', '0B875E9D-0EE5-40EF-9CC6-451259EEADB3', N'تمرين: مسح الجسد للاسترخاء (Body Scan Relaxation) - تمرين وعي جسدي تدريجي من القدمين إلى الرأس.', N'Daily', '2026-01-21', '2026-03-02', 15, N'Medium', N'Completed', '2026-01-21 12:00:00', '2026-01-21 12:00:00'),
('0272206C-5015-4854-901E-CCCED94768D8', '0B875E9D-0EE5-40EF-9CC6-451259EEADB3', N'تمرين: تمرين وقت القلق المحدد (Worry Time Exercise) - تخصيص 15 دقيقة يومياً فقط للتفكير في المخاوف.', N'Daily', '2026-01-20', '2026-04-04', 15, N'Medium', N'Completed', '2026-01-20 12:00:00', '2026-01-20 12:00:00'),
('2FC27164-204F-4FAE-AF3E-A57E16FBD34A', '0B875E9D-0EE5-40EF-9CC6-451259EEADB3', N'تمرين: الاسترخاء العضلي التدريجي (Progressive Muscle Relaxation) - شد وإرخاء مجموعات العضلات تباعاً لتقليل التوتر الجسدي.', N'Weekly', '2026-01-24', '2026-03-22', 20, N'Medium', N'Completed', '2026-01-24 12:00:00', '2026-01-24 12:00:00'),
('4FF196B4-8097-487A-97BC-D11E035B33DB', '0B875E9D-0EE5-40EF-9CC6-451259EEADB3', N'تمرين: تقنية التأريض 5-4-3-2-1 (Grounding Technique) - تمرين حسي لإعادة التركيز إلى اللحظة الحالية.', N'Biweekly', '2026-01-26', '2026-04-17', 15, N'Easy', N'Completed', '2026-01-26 12:00:00', '2026-01-26 12:00:00'),
('1532868E-71BC-489A-B1BA-A958F0F98EC0', 'ED4A6B23-9515-4C85-9110-1C4CC21FAA9E', N'تمرين: تمرين وقت القلق المحدد (Worry Time Exercise) - تخصيص 15 دقيقة يومياً فقط للتفكير في المخاوف.', N'Weekly', '2026-01-17', '2026-03-04', 15, N'Medium', N'Completed', '2026-01-17 12:00:00', '2026-01-17 12:00:00'),
('9A0F4CB3-9DCE-4EA0-8740-3D93108CBB57', 'ED4A6B23-9515-4C85-9110-1C4CC21FAA9E', N'تمرين: يوميات الامتنان (Gratitude Journal) - تدوين ثلاثة أشياء إيجابية حدثت خلال اليوم.', N'Daily', '2026-01-16', '2026-04-10', 10, N'Easy', N'Completed', '2026-01-16 12:00:00', '2026-01-16 12:00:00'),
('63043732-ACF6-4C23-BB1D-740E5022F3E3', 'ED4A6B23-9515-4C85-9110-1C4CC21FAA9E', N'تمرين: تتبع المزاج اليومي (Mood Tracking) - تسجيل الحالة المزاجية ثلاث مرات يومياً.', N'Biweekly', '2026-01-15', '2026-03-21', 5, N'Easy', N'Completed', '2026-01-15 12:00:00', '2026-01-15 12:00:00'),
('F55D45BA-36D7-4A39-9EC4-9B86C3C3A7C8', 'ED4A6B23-9515-4C85-9110-1C4CC21FAA9E', N'تمرين: قائمة نظافة النوم (Sleep Hygiene Checklist) - مراجعة عادات ما قبل النوم لتحسين جودة النوم.', N'Biweekly', '2026-01-16', '2026-02-23', 10, N'Easy', N'Completed', '2026-01-16 12:00:00', '2026-01-16 12:00:00'),
('E4CF0A16-C6DE-4B55-8AFA-46A17160D3F7', '69F104EE-9B40-40CF-A029-EC2A532693FA', N'تمرين: الحديث الذاتي الإيجابي (Positive Self Talk) - استبدال الأفكار السلبية بعبارات داعمة للذات.', N'Biweekly', '2026-04-15', '2026-06-17', 10, N'Medium', N'Completed', '2026-04-15 12:00:00', '2026-04-15 12:00:00'),
('D1686C83-CDED-4630-9980-0FAC3BED7BE3', '69F104EE-9B40-40CF-A029-EC2A532693FA', N'تمرين: تمرين وقت القلق المحدد (Worry Time Exercise) - تخصيص 15 دقيقة يومياً فقط للتفكير في المخاوف.', N'Daily', '2026-04-12', '2026-06-07', 15, N'Medium', N'Completed', '2026-04-12 12:00:00', '2026-04-12 12:00:00'),
('10B8BCC6-629F-4A0C-B704-1F70570E3349', '69F104EE-9B40-40CF-A029-EC2A532693FA', N'تمرين: التأمل الموجه (Guided Meditation) - جلسة تأمل صوتية موجهة لتقليل التوتر.', N'Weekly', '2026-04-11', '2026-05-14', 20, N'Medium', N'Completed', '2026-04-11 12:00:00', '2026-04-11 12:00:00'),
('7D9357C8-7140-45F4-961C-33FE9FA9D76F', '56A8CAE5-A2FA-4149-B2D0-3E5B35F37D3B', N'تمرين: الاسترخاء العضلي التدريجي (Progressive Muscle Relaxation) - شد وإرخاء مجموعات العضلات تباعاً لتقليل التوتر الجسدي.', N'Daily', '2026-05-05', '2026-06-11', 20, N'Medium', N'Completed', '2026-05-05 12:00:00', '2026-05-05 12:00:00'),
('930AB235-378D-483A-9FE8-F603BF555724', '56A8CAE5-A2FA-4149-B2D0-3E5B35F37D3B', N'تمرين: قائمة نظافة النوم (Sleep Hygiene Checklist) - مراجعة عادات ما قبل النوم لتحسين جودة النوم.', N'Daily', '2026-05-02', '2026-06-28', 10, N'Easy', N'Completed', '2026-05-02 12:00:00', '2026-05-02 12:00:00'),
('4CFE6AD8-547A-42E4-BBDF-1EA89AE328E4', '56A8CAE5-A2FA-4149-B2D0-3E5B35F37D3B', N'تمرين: تقنية التأريض 5-4-3-2-1 (Grounding Technique) - تمرين حسي لإعادة التركيز إلى اللحظة الحالية.', N'Weekly', '2026-05-04', '2026-07-02', 15, N'Easy', N'Completed', '2026-05-04 12:00:00', '2026-05-04 12:00:00'),
('2448A917-391B-4F95-BF78-E5F0186D8B66', '56A8CAE5-A2FA-4149-B2D0-3E5B35F37D3B', N'تمرين: تتبع المزاج اليومي (Mood Tracking) - تسجيل الحالة المزاجية ثلاث مرات يومياً.', N'Biweekly', '2026-05-03', '2026-06-26', 5, N'Easy', N'Completed', '2026-05-03 12:00:00', '2026-05-03 12:00:00'),
('130124E3-2CC6-4E58-BDE1-EE6A36C5EFCA', '85B46DDD-B1A6-4148-BB18-D7DB4172EAF2', N'تمرين: قائمة نظافة النوم (Sleep Hygiene Checklist) - مراجعة عادات ما قبل النوم لتحسين جودة النوم.', N'Weekly', '2025-09-14', '2025-10-23', 10, N'Easy', N'Completed', '2025-09-14 12:00:00', '2025-09-14 12:00:00'),
('7E63C494-9958-4245-A639-C034022DFB13', '85B46DDD-B1A6-4148-BB18-D7DB4172EAF2', N'تمرين: الحديث الذاتي الإيجابي (Positive Self Talk) - استبدال الأفكار السلبية بعبارات داعمة للذات.', N'Biweekly', '2025-09-16', '2025-10-20', 10, N'Medium', N'Abandoned', '2025-09-16 12:00:00', '2025-09-16 12:00:00'),
('58A102E0-817C-4EA4-88FB-4ACA0D89AC21', 'D5AF7A7D-26C9-4CD6-854B-11C4F6A65218', N'تمرين: تقنية التأريض 5-4-3-2-1 (Grounding Technique) - تمرين حسي لإعادة التركيز إلى اللحظة الحالية.', N'Biweekly', '2025-10-08', '2025-11-11', 15, N'Easy', N'Completed', '2025-10-08 12:00:00', '2025-10-08 12:00:00'),
('01345114-20DE-4806-A6D4-2E9FB2011EF3', 'D5AF7A7D-26C9-4CD6-854B-11C4F6A65218', N'تمرين: سجل الأفكار (Thought Record - CBT) - تحديد الأفكار التلقائية السلبية وإعادة صياغتها منطقياً.', N'Weekly', '2025-10-08', '2025-11-07', 25, N'Hard', N'Completed', '2025-10-08 12:00:00', '2025-10-08 12:00:00'),
('638930A8-25A5-4C0E-A532-04D209CF79D2', 'D5AF7A7D-26C9-4CD6-854B-11C4F6A65218', N'تمرين: التنفس العميق (Deep Breathing) - تمرين تنفس بطيء وعميق لتهدئة الجهاز العصبي.', N'Weekly', '2025-10-05', '2025-11-07', 10, N'Easy', N'Completed', '2025-10-05 12:00:00', '2025-10-05 12:00:00'),
('FB6CC26C-CD98-4F17-A02F-6A47A417D98F', 'D5AF7A7D-26C9-4CD6-854B-11C4F6A65218', N'تمرين: مسح الجسد للاسترخاء (Body Scan Relaxation) - تمرين وعي جسدي تدريجي من القدمين إلى الرأس.', N'Weekly', '2025-10-05', '2025-12-08', 15, N'Medium', N'Completed', '2025-10-05 12:00:00', '2025-10-05 12:00:00'),
('1A70C056-4875-4085-B56C-4F1F95E1EAA3', 'AEB38627-5539-4776-8AEC-69F91DD159F1', N'تمرين: سجل الأفكار (Thought Record - CBT) - تحديد الأفكار التلقائية السلبية وإعادة صياغتها منطقياً.', N'Daily', '2025-11-18', '2025-12-28', 25, N'Hard', N'Completed', '2025-11-18 12:00:00', '2025-11-18 12:00:00'),
('A238C47A-F5DC-4ECF-905B-77F2972113F5', 'AEB38627-5539-4776-8AEC-69F91DD159F1', N'تمرين: تتبع المزاج اليومي (Mood Tracking) - تسجيل الحالة المزاجية ثلاث مرات يومياً.', N'Daily', '2025-11-21', '2026-01-01', 5, N'Easy', N'Completed', '2025-11-21 12:00:00', '2025-11-21 12:00:00'),
('78A3DC62-30C3-4D07-8420-686EB7F8E71B', 'AEB38627-5539-4776-8AEC-69F91DD159F1', N'تمرين: سلم التعرض الاجتماعي (Social Exposure Ladder) - التعرض التدريجي لمواقف اجتماعية مثيرة للقلق.', N'Daily', '2025-11-17', '2026-02-06', 30, N'Hard', N'Completed', '2025-11-17 12:00:00', '2025-11-17 12:00:00'),
('6CF486F3-E92B-4AD0-861F-61099A4ADF90', 'AEB38627-5539-4776-8AEC-69F91DD159F1', N'تمرين: يوميات التأمل في الضغوط (Stress Reflection Journal) - تدوين مصادر الضغط اليومية وطرق التعامل معها.', N'Weekly', '2025-11-20', '2026-01-22', 15, N'Medium', N'Completed', '2025-11-20 12:00:00', '2025-11-20 12:00:00'),
('293E35A0-25B0-43B6-9574-709F14FE8564', '00CC925F-9671-4970-ADC6-5C26F8FBF8E5', N'تمرين: تمرين وقت القلق المحدد (Worry Time Exercise) - تخصيص 15 دقيقة يومياً فقط للتفكير في المخاوف.', N'Daily', '2026-04-05', '2026-06-13', 15, N'Medium', N'Completed', '2026-04-05 12:00:00', '2026-04-05 12:00:00'),
('4D335F31-F0BE-4982-9D4B-5CE701FF4127', '00CC925F-9671-4970-ADC6-5C26F8FBF8E5', N'تمرين: سجل نوبات الغضب (Anger Log) - تسجيل مواقف الغضب ومحفزاتها واستجابات التعامل معها.', N'Biweekly', '2026-04-10', '2026-05-12', 10, N'Medium', N'Completed', '2026-04-10 12:00:00', '2026-04-10 12:00:00'),
('A71FB1CE-D2CC-4DA3-AB76-2A00E5BE2BD6', '5947B1E9-EACB-4CD1-A014-69DBDA364362', N'تمرين: مسح الجسد للاسترخاء (Body Scan Relaxation) - تمرين وعي جسدي تدريجي من القدمين إلى الرأس.', N'Daily', '2025-12-15', '2026-02-19', 15, N'Medium', N'Completed', '2025-12-15 12:00:00', '2025-12-15 12:00:00'),
('4DDC2FC3-5D35-4410-8FFC-B74F604D909D', '5947B1E9-EACB-4CD1-A014-69DBDA364362', N'تمرين: يوميات الامتنان (Gratitude Journal) - تدوين ثلاثة أشياء إيجابية حدثت خلال اليوم.', N'Weekly', '2025-12-11', '2026-01-24', 10, N'Easy', N'Completed', '2025-12-11 12:00:00', '2025-12-11 12:00:00'),
('993F4F3F-FEBA-453B-9226-1F159001EC78', '5947B1E9-EACB-4CD1-A014-69DBDA364362', N'تمرين: سجل الأفكار (Thought Record - CBT) - تحديد الأفكار التلقائية السلبية وإعادة صياغتها منطقياً.', N'Weekly', '2025-12-13', '2026-01-12', 25, N'Hard', N'Abandoned', '2025-12-13 12:00:00', '2025-12-13 12:00:00'),
('5FBC3CF4-B008-4DC9-8AB1-1182FF5757EB', 'D05B32B7-0D94-4041-91CE-8952FF6C525A', N'تمرين: تمرين وقت القلق المحدد (Worry Time Exercise) - تخصيص 15 دقيقة يومياً فقط للتفكير في المخاوف.', N'Daily', '2025-07-25', '2025-10-11', 15, N'Medium', N'Completed', '2025-07-25 12:00:00', '2025-07-25 12:00:00'),
('23AA7606-5E11-4DDB-9DFB-DF597875D972', 'D05B32B7-0D94-4041-91CE-8952FF6C525A', N'تمرين: الحديث الذاتي الإيجابي (Positive Self Talk) - استبدال الأفكار السلبية بعبارات داعمة للذات.', N'Biweekly', '2025-07-24', '2025-09-28', 10, N'Medium', N'Abandoned', '2025-07-24 12:00:00', '2025-07-24 12:00:00'),
('89455528-3F07-4F36-83C3-97D4A1596C91', '7F9C808B-66FB-44EC-AE2F-D987CCB0D98A', N'تمرين: تتبع المزاج اليومي (Mood Tracking) - تسجيل الحالة المزاجية ثلاث مرات يومياً.', N'Daily', '2026-03-01', '2026-04-05', 5, N'Easy', N'Completed', '2026-03-01 12:00:00', '2026-03-01 12:00:00'),
('C557AA9B-74D3-46BA-9A70-F4E87263ED95', '7F9C808B-66FB-44EC-AE2F-D987CCB0D98A', N'تمرين: سلم التعرض الاجتماعي (Social Exposure Ladder) - التعرض التدريجي لمواقف اجتماعية مثيرة للقلق.', N'Daily', '2026-02-26', '2026-05-09', 30, N'Hard', N'Completed', '2026-02-26 12:00:00', '2026-02-26 12:00:00'),
('0E588A0A-0244-4BD6-BBC3-FBF133CB82A8', '7F9C808B-66FB-44EC-AE2F-D987CCB0D98A', N'تمرين: مسح الجسد للاسترخاء (Body Scan Relaxation) - تمرين وعي جسدي تدريجي من القدمين إلى الرأس.', N'Daily', '2026-02-22', '2026-05-09', 15, N'Medium', N'Completed', '2026-02-22 12:00:00', '2026-02-22 12:00:00'),
('E2F445FC-D55F-47B0-9E3C-0974847057C8', '080C5383-4556-405D-BBEC-38C843811EBD', N'تمرين: استراحة الرأفة الذاتية (Self-Compassion Break) - تمرين قصير للتعامل مع الذات بلطف في أوقات الضغط.', N'Daily', '2026-07-06', '2026-09-21', 10, N'Easy', N'Active', '2026-07-06 12:00:00', '2026-07-06 12:00:00'),
('57D6AD95-1CD4-4057-BFEE-1C492E041297', '080C5383-4556-405D-BBEC-38C843811EBD', N'تمرين: التأمل الموجه (Guided Meditation) - جلسة تأمل صوتية موجهة لتقليل التوتر.', N'Daily', '2026-07-05', '2026-08-24', 20, N'Medium', N'Active', '2026-07-05 12:00:00', '2026-07-05 12:00:00'),
('11E2C6DB-E6D0-4FDB-8CF2-44A6409EE413', '080C5383-4556-405D-BBEC-38C843811EBD', N'تمرين: تقنية التأريض 5-4-3-2-1 (Grounding Technique) - تمرين حسي لإعادة التركيز إلى اللحظة الحالية.', N'Biweekly', '2026-07-08', '2026-08-25', 15, N'Easy', N'Active', '2026-07-08 12:00:00', '2026-07-08 12:00:00'),
('05E08E6C-2A93-4056-8768-6028A22C119A', '5A0DA59E-4AE0-4CF6-B7CD-5BDB2F91D5E7', N'تمرين: سجل الأفكار (Thought Record - CBT) - تحديد الأفكار التلقائية السلبية وإعادة صياغتها منطقياً.', N'Biweekly', '2026-01-29', '2026-04-08', 25, N'Hard', N'Abandoned', '2026-01-29 12:00:00', '2026-01-29 12:00:00'),
('09D031EA-1776-420A-8C1B-EB00D86C3BF4', '5A0DA59E-4AE0-4CF6-B7CD-5BDB2F91D5E7', N'تمرين: استراحة الرأفة الذاتية (Self-Compassion Break) - تمرين قصير للتعامل مع الذات بلطف في أوقات الضغط.', N'Daily', '2026-02-01', '2026-04-03', 10, N'Easy', N'Completed', '2026-02-01 12:00:00', '2026-02-01 12:00:00'),
('6C48C212-2942-4A82-B5FB-ED315FCEE56E', '5A0DA59E-4AE0-4CF6-B7CD-5BDB2F91D5E7', N'تمرين: يوميات التأمل في الضغوط (Stress Reflection Journal) - تدوين مصادر الضغط اليومية وطرق التعامل معها.', N'Weekly', '2026-02-05', '2026-03-25', 15, N'Medium', N'Completed', '2026-02-05 12:00:00', '2026-02-05 12:00:00'),
('7E4618E7-E6EC-4C84-B1DD-4179999269D5', '0004E60D-C638-4639-BCFA-0AD58724C84E', N'تمرين: سجل الأفكار (Thought Record - CBT) - تحديد الأفكار التلقائية السلبية وإعادة صياغتها منطقياً.', N'Daily', '2026-06-24', '2026-08-25', 25, N'Hard', N'Active', '2026-06-24 12:00:00', '2026-06-24 12:00:00'),
('23FB3941-962F-4EAF-B853-24978618CB99', '0004E60D-C638-4639-BCFA-0AD58724C84E', N'تمرين: سلم التعرض الاجتماعي (Social Exposure Ladder) - التعرض التدريجي لمواقف اجتماعية مثيرة للقلق.', N'Weekly', '2026-06-24', '2026-09-07', 30, N'Hard', N'Active', '2026-06-24 12:00:00', '2026-06-24 12:00:00'),
('BFC9DD17-D408-4BA7-BA0D-3E088DCE86B0', '0004E60D-C638-4639-BCFA-0AD58724C84E', N'تمرين: التأمل الموجه (Guided Meditation) - جلسة تأمل صوتية موجهة لتقليل التوتر.', N'Daily', '2026-06-23', '2026-07-28', 20, N'Medium', N'Active', '2026-06-23 12:00:00', '2026-06-23 12:00:00'),
('9AA8F5D7-D289-4398-B65D-ACAED2136578', '81B640AC-DBBD-4F29-831E-F36B5C6A7580', N'تمرين: التنفس العميق (Deep Breathing) - تمرين تنفس بطيء وعميق لتهدئة الجهاز العصبي.', N'Weekly', '2026-03-12', '2026-05-20', 10, N'Easy', N'Completed', '2026-03-12 12:00:00', '2026-03-12 12:00:00'),
('EB2AAC9D-944F-4558-9F3C-FC0724E6F762', '81B640AC-DBBD-4F29-831E-F36B5C6A7580', N'تمرين: الحديث الذاتي الإيجابي (Positive Self Talk) - استبدال الأفكار السلبية بعبارات داعمة للذات.', N'Biweekly', '2026-03-10', '2026-04-26', 10, N'Medium', N'Abandoned', '2026-03-10 12:00:00', '2026-03-10 12:00:00'),
('B3E24B27-5A98-4EED-8350-E6AE24630879', '85C8A7BC-BBE9-4D03-B8D2-89AFEE304102', N'تمرين: قائمة نظافة النوم (Sleep Hygiene Checklist) - مراجعة عادات ما قبل النوم لتحسين جودة النوم.', N'Weekly', '2026-02-18', '2026-03-20', 10, N'Easy', N'Completed', '2026-02-18 12:00:00', '2026-02-18 12:00:00'),
('6572D047-F317-423C-9F84-A19D5D937565', '85C8A7BC-BBE9-4D03-B8D2-89AFEE304102', N'تمرين: سجل الأفكار (Thought Record - CBT) - تحديد الأفكار التلقائية السلبية وإعادة صياغتها منطقياً.', N'Daily', '2026-02-12', '2026-03-20', 25, N'Hard', N'Completed', '2026-02-12 12:00:00', '2026-02-12 12:00:00'),
('6CD79544-698B-4BB9-8AC6-4604AEBC000B', '85C8A7BC-BBE9-4D03-B8D2-89AFEE304102', N'تمرين: سلم التعرض الاجتماعي (Social Exposure Ladder) - التعرض التدريجي لمواقف اجتماعية مثيرة للقلق.', N'Weekly', '2026-02-13', '2026-03-30', 30, N'Hard', N'Completed', '2026-02-13 12:00:00', '2026-02-13 12:00:00'),
('1832C622-71AD-44AE-ACD3-63A1AE2B8FCF', '46ACC094-18A3-4CCF-80C4-95D298EF4031', N'تمرين: الحديث الذاتي الإيجابي (Positive Self Talk) - استبدال الأفكار السلبية بعبارات داعمة للذات.', N'Weekly', '2026-02-27', '2026-04-27', 10, N'Medium', N'Abandoned', '2026-02-27 12:00:00', '2026-02-27 12:00:00'),
('AD1CD297-85B1-4F97-B8B4-943BA34D87F7', '46ACC094-18A3-4CCF-80C4-95D298EF4031', N'تمرين: قائمة نظافة النوم (Sleep Hygiene Checklist) - مراجعة عادات ما قبل النوم لتحسين جودة النوم.', N'Weekly', '2026-02-23', '2026-05-03', 10, N'Easy', N'Abandoned', '2026-02-23 12:00:00', '2026-02-23 12:00:00'),
('DE5290E1-489C-4A2D-B374-508E7B67BBE0', '46ACC094-18A3-4CCF-80C4-95D298EF4031', N'تمرين: تقنية التأريض 5-4-3-2-1 (Grounding Technique) - تمرين حسي لإعادة التركيز إلى اللحظة الحالية.', N'Daily', '2026-02-28', '2026-04-02', 15, N'Easy', N'Completed', '2026-02-28 12:00:00', '2026-02-28 12:00:00'),
('996204F7-5016-4EA4-ABC7-2ED33C858D58', 'F300A791-B73E-4A86-90EA-4D1EE67BBE48', N'تمرين: التنفس العميق (Deep Breathing) - تمرين تنفس بطيء وعميق لتهدئة الجهاز العصبي.', N'Daily', '2026-06-05', '2026-07-09', 10, N'Easy', N'Active', '2026-06-05 12:00:00', '2026-06-05 12:00:00'),
('C5FC3C51-7F40-4B7B-81D5-5BAB80739E81', 'F300A791-B73E-4A86-90EA-4D1EE67BBE48', N'تمرين: تقنية التأريض 5-4-3-2-1 (Grounding Technique) - تمرين حسي لإعادة التركيز إلى اللحظة الحالية.', N'Daily', '2026-06-11', '2026-07-25', 15, N'Easy', N'Active', '2026-06-11 12:00:00', '2026-06-11 12:00:00'),
('ECDD0990-1553-4F55-AFA4-4BF09AB009ED', 'F300A791-B73E-4A86-90EA-4D1EE67BBE48', N'تمرين: تتبع المزاج اليومي (Mood Tracking) - تسجيل الحالة المزاجية ثلاث مرات يومياً.', N'Daily', '2026-06-06', '2026-08-12', 5, N'Easy', N'Active', '2026-06-06 12:00:00', '2026-06-06 12:00:00'),
('0196B6A9-6360-40A4-99DC-E286F7BEB01D', 'F300A791-B73E-4A86-90EA-4D1EE67BBE48', N'تمرين: مسح الجسد للاسترخاء (Body Scan Relaxation) - تمرين وعي جسدي تدريجي من القدمين إلى الرأس.', N'Daily', '2026-06-05', '2026-08-16', 15, N'Medium', N'Active', '2026-06-05 12:00:00', '2026-06-05 12:00:00'),
('8F19F2C4-4B41-40F6-B508-EF9B0298AD82', '74FDAC62-9C1A-472B-AA93-BCDC4103514D', N'تمرين: يوميات التأمل في الضغوط (Stress Reflection Journal) - تدوين مصادر الضغط اليومية وطرق التعامل معها.', N'Weekly', '2026-01-24', '2026-04-06', 15, N'Medium', N'Completed', '2026-01-24 12:00:00', '2026-01-24 12:00:00'),
('E6A9D146-0B6F-4F18-857F-75F0AB47A87D', '74FDAC62-9C1A-472B-AA93-BCDC4103514D', N'تمرين: تمرين وقت القلق المحدد (Worry Time Exercise) - تخصيص 15 دقيقة يومياً فقط للتفكير في المخاوف.', N'Weekly', '2026-01-26', '2026-03-22', 15, N'Medium', N'Completed', '2026-01-26 12:00:00', '2026-01-26 12:00:00'),
('8CE11432-F192-4F06-AF8E-E16AC00D0CDC', '5A2EBBD8-21B9-47E7-881C-B09F16F70FE6', N'تمرين: سجل نوبات الغضب (Anger Log) - تسجيل مواقف الغضب ومحفزاتها واستجابات التعامل معها.', N'Weekly', '2026-06-06', '2026-08-05', 10, N'Medium', N'Active', '2026-06-06 12:00:00', '2026-06-06 12:00:00'),
('49B2FED3-64E9-496A-A2F4-3339277814E4', '5A2EBBD8-21B9-47E7-881C-B09F16F70FE6', N'تمرين: يوميات التأمل في الضغوط (Stress Reflection Journal) - تدوين مصادر الضغط اليومية وطرق التعامل معها.', N'Biweekly', '2026-06-08', '2026-08-10', 15, N'Medium', N'Active', '2026-06-08 12:00:00', '2026-06-08 12:00:00'),
('B253C21C-01F6-478E-8343-ACC9F40815DB', '5A2EBBD8-21B9-47E7-881C-B09F16F70FE6', N'تمرين: مسح الجسد للاسترخاء (Body Scan Relaxation) - تمرين وعي جسدي تدريجي من القدمين إلى الرأس.', N'Daily', '2026-06-11', '2026-07-12', 15, N'Medium', N'Active', '2026-06-11 12:00:00', '2026-06-11 12:00:00'),
('EA3C0CD0-E803-4F90-99D4-D1BB22690EA6', '5A2EBBD8-21B9-47E7-881C-B09F16F70FE6', N'تمرين: استراحة الرأفة الذاتية (Self-Compassion Break) - تمرين قصير للتعامل مع الذات بلطف في أوقات الضغط.', N'Weekly', '2026-06-12', '2026-07-10', 10, N'Easy', N'Active', '2026-06-12 12:00:00', '2026-06-12 12:00:00'),
('7F6DBE59-0763-440F-9A4A-467F6A1970BD', 'CC37E596-7078-4E46-938E-D6B59CA67B02', N'تمرين: سجل الأفكار (Thought Record - CBT) - تحديد الأفكار التلقائية السلبية وإعادة صياغتها منطقياً.', N'Daily', '2026-03-20', '2026-05-14', 25, N'Hard', N'Completed', '2026-03-20 12:00:00', '2026-03-20 12:00:00'),
('8FF7B388-D36B-4D71-935E-C325F10B317C', 'CC37E596-7078-4E46-938E-D6B59CA67B02', N'تمرين: مسح الجسد للاسترخاء (Body Scan Relaxation) - تمرين وعي جسدي تدريجي من القدمين إلى الرأس.', N'Daily', '2026-03-20', '2026-04-19', 15, N'Medium', N'Completed', '2026-03-20 12:00:00', '2026-03-20 12:00:00'),
('DF3507EA-8CE2-4E5A-9CE2-43B4BBBFF4CD', 'CC37E596-7078-4E46-938E-D6B59CA67B02', N'تمرين: التأمل الموجه (Guided Meditation) - جلسة تأمل صوتية موجهة لتقليل التوتر.', N'Weekly', '2026-03-16', '2026-04-15', 20, N'Medium', N'Completed', '2026-03-16 12:00:00', '2026-03-16 12:00:00'),
('638A5E2A-D953-4F46-B5F1-B8F1CCB23F67', 'CC37E596-7078-4E46-938E-D6B59CA67B02', N'تمرين: التنفس العميق (Deep Breathing) - تمرين تنفس بطيء وعميق لتهدئة الجهاز العصبي.', N'Biweekly', '2026-03-19', '2026-05-13', 10, N'Easy', N'Completed', '2026-03-19 12:00:00', '2026-03-19 12:00:00'),
('C07977DB-A6B5-432F-8A47-D0566310210A', '5F6B076C-526F-4F65-B5E3-F416520D1841', N'تمرين: الحديث الذاتي الإيجابي (Positive Self Talk) - استبدال الأفكار السلبية بعبارات داعمة للذات.', N'Weekly', '2026-03-15', '2026-05-26', 10, N'Medium', N'Completed', '2026-03-15 12:00:00', '2026-03-15 12:00:00'),
('815C74CD-0F24-4C5E-A8D2-327F78507FE2', '5F6B076C-526F-4F65-B5E3-F416520D1841', N'تمرين: مسح الجسد للاسترخاء (Body Scan Relaxation) - تمرين وعي جسدي تدريجي من القدمين إلى الرأس.', N'Biweekly', '2026-03-15', '2026-05-01', 15, N'Medium', N'Abandoned', '2026-03-15 12:00:00', '2026-03-15 12:00:00'),
('033CF2A8-DD98-45DF-81F0-A078E05427D3', '5F6B076C-526F-4F65-B5E3-F416520D1841', N'تمرين: استراحة الرأفة الذاتية (Self-Compassion Break) - تمرين قصير للتعامل مع الذات بلطف في أوقات الضغط.', N'Biweekly', '2026-03-15', '2026-05-12', 10, N'Easy', N'Abandoned', '2026-03-15 12:00:00', '2026-03-15 12:00:00'),
('E8547BAA-F924-4FEA-9974-0A72A3FF1BB9', '5F6B076C-526F-4F65-B5E3-F416520D1841', N'تمرين: التنفس العميق (Deep Breathing) - تمرين تنفس بطيء وعميق لتهدئة الجهاز العصبي.', N'Weekly', '2026-03-17', '2026-04-27', 10, N'Easy', N'Completed', '2026-03-17 12:00:00', '2026-03-17 12:00:00'),
('718342AD-DE69-42A4-8FD5-EC6FA6C1AAA5', '8BDD5425-225E-4657-AF47-CA87A9625781', N'تمرين: تتبع المزاج اليومي (Mood Tracking) - تسجيل الحالة المزاجية ثلاث مرات يومياً.', N'Weekly', '2025-11-25', '2025-12-27', 5, N'Easy', N'Completed', '2025-11-25 12:00:00', '2025-11-25 12:00:00'),
('EA90646C-E097-4CF8-82AA-CFD27F8D743C', '8BDD5425-225E-4657-AF47-CA87A9625781', N'تمرين: يوميات الامتنان (Gratitude Journal) - تدوين ثلاثة أشياء إيجابية حدثت خلال اليوم.', N'Weekly', '2025-11-28', '2026-02-11', 10, N'Easy', N'Completed', '2025-11-28 12:00:00', '2025-11-28 12:00:00'),
('B17D7BEF-A5B7-4C08-851D-94DE56C45652', '8BDD5425-225E-4657-AF47-CA87A9625781', N'تمرين: سجل نوبات الغضب (Anger Log) - تسجيل مواقف الغضب ومحفزاتها واستجابات التعامل معها.', N'Daily', '2025-11-25', '2026-02-14', 10, N'Medium', N'Abandoned', '2025-11-25 12:00:00', '2025-11-25 12:00:00'),
('677D9929-6814-4C1A-A5AF-95198D73A66A', 'D149A229-6389-48F8-91B0-BE1D2E2900B1', N'تمرين: الاسترخاء العضلي التدريجي (Progressive Muscle Relaxation) - شد وإرخاء مجموعات العضلات تباعاً لتقليل التوتر الجسدي.', N'Weekly', '2025-09-12', '2025-10-12', 20, N'Medium', N'Completed', '2025-09-12 12:00:00', '2025-09-12 12:00:00'),
('1BAD9CCD-512F-4199-8843-941D75781975', 'D149A229-6389-48F8-91B0-BE1D2E2900B1', N'تمرين: تمرين وقت القلق المحدد (Worry Time Exercise) - تخصيص 15 دقيقة يومياً فقط للتفكير في المخاوف.', N'Weekly', '2025-09-06', '2025-10-21', 15, N'Medium', N'Completed', '2025-09-06 12:00:00', '2025-09-06 12:00:00'),
('C02A2BFC-6B2C-418F-8C62-73F17342AD0B', 'D149A229-6389-48F8-91B0-BE1D2E2900B1', N'تمرين: التأمل الموجه (Guided Meditation) - جلسة تأمل صوتية موجهة لتقليل التوتر.', N'Weekly', '2025-09-07', '2025-11-27', 20, N'Medium', N'Completed', '2025-09-07 12:00:00', '2025-09-07 12:00:00');

-- ===== ExerciseLogs =====
INSERT INTO [ExerciseLogs] ([Id], [ExerciseId], [PatientId], [CompletionStatus], [ReflectionNote], [MoodBefore], [MoodAfter], [LoggedAt], [CreatedAt])
VALUES
('CEAA098D-D45D-4894-B56F-F271ECE0674D', 'E13B4964-28FD-4FA1-89A8-D29488FBEB82', '5912AA70-D127-4B2A-95B6-62222A464244', N'Partial', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 3, 3, '2026-05-27 00:18:00', '2026-05-26 12:00:00'),
('8F2AF92D-1690-480A-A5F9-C014E92E67B0', 'E13B4964-28FD-4FA1-89A8-D29488FBEB82', '5912AA70-D127-4B2A-95B6-62222A464244', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 4, 6, '2026-06-01 09:19:00', '2026-05-31 12:00:00'),
('BF57385C-C310-4EF9-BC88-57A660CE1F98', 'E13B4964-28FD-4FA1-89A8-D29488FBEB82', '5912AA70-D127-4B2A-95B6-62222A464244', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 4, 7, '2026-06-06 09:07:00', '2026-06-05 12:00:00'),
('037B6DEB-3118-487C-96EC-1164A4514EE2', 'E13B4964-28FD-4FA1-89A8-D29488FBEB82', '5912AA70-D127-4B2A-95B6-62222A464244', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 5, 7, '2026-06-11 06:33:00', '2026-06-10 12:00:00'),
('0DFD5290-9EDB-4FC5-AB6A-817E66CC8A4B', 'E13B4964-28FD-4FA1-89A8-D29488FBEB82', '5912AA70-D127-4B2A-95B6-62222A464244', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 6, 8, '2026-06-16 22:58:00', '2026-06-16 12:00:00'),
('784426E3-6BA7-45D5-A2E8-55B1BAC09A31', 'E13B4964-28FD-4FA1-89A8-D29488FBEB82', '5912AA70-D127-4B2A-95B6-62222A464244', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 4, 7, '2026-06-22 04:51:00', '2026-06-21 12:00:00'),
('E8923424-40E0-4C8C-974D-074E748D4F6B', 'E13B4964-28FD-4FA1-89A8-D29488FBEB82', '5912AA70-D127-4B2A-95B6-62222A464244', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 5, 7, '2026-06-26 21:28:00', '2026-06-26 12:00:00'),
('65E74222-9C56-472C-B743-08501F9565CE', 'E13B4964-28FD-4FA1-89A8-D29488FBEB82', '5912AA70-D127-4B2A-95B6-62222A464244', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 7, 9, '2026-07-02 09:37:00', '2026-07-01 12:00:00'),
('B7C2FB4A-C1E5-4C79-B77F-C0F0DD151BF4', '8F00AD90-507D-4558-83AE-9BEA3253936E', '5912AA70-D127-4B2A-95B6-62222A464244', N'Partial', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 3, 4, '2026-05-23 23:40:00', '2026-05-23 12:00:00'),
('E72EA1EE-9F9F-467E-B78F-A9EB85FA714E', '8F00AD90-507D-4558-83AE-9BEA3253936E', '5912AA70-D127-4B2A-95B6-62222A464244', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 5, 7, '2026-06-02 02:51:00', '2026-06-01 12:00:00'),
('2E5691C7-1720-4E5B-9924-42EFB5A57E75', '8F00AD90-507D-4558-83AE-9BEA3253936E', '5912AA70-D127-4B2A-95B6-62222A464244', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 4, 8, '2026-06-11 09:08:00', '2026-06-10 12:00:00'),
('CC6395BB-46AC-4A13-A774-FB76BC30889B', '8F00AD90-507D-4558-83AE-9BEA3253936E', '5912AA70-D127-4B2A-95B6-62222A464244', N'Partial', N'شعرت بهدوء أكبر بعد التمرين.', 4, 6, '2026-06-19 04:08:00', '2026-06-18 12:00:00'),
('4D1B469D-9DAA-4E7C-BB1D-C6ED192E8A86', '8F00AD90-507D-4558-83AE-9BEA3253936E', '5912AA70-D127-4B2A-95B6-62222A464244', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 6, 10, '2026-06-28 01:19:00', '2026-06-27 12:00:00'),
('77091AFA-3199-4B3A-8983-D57B20AF4250', '8F00AD90-507D-4558-83AE-9BEA3253936E', '5912AA70-D127-4B2A-95B6-62222A464244', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 5, 9, '2026-07-07 00:32:00', '2026-07-06 12:00:00'),
('53731641-CDDD-400C-8BF1-E788B86626E8', '51398AA0-9388-4596-AC24-594A512CC0B5', 'D091A7F4-A73A-4CCE-920E-0AB7D4C36536', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 2, 5, '2025-07-03 23:53:00', '2025-07-03 12:00:00'),
('77FAF1A9-F4E0-4E32-8D93-35FE2B775ECA', '51398AA0-9388-4596-AC24-594A512CC0B5', 'D091A7F4-A73A-4CCE-920E-0AB7D4C36536', N'Partial', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 4, 4, '2025-07-09 22:33:00', '2025-07-09 12:00:00'),
('DAE4F25D-9962-45C9-8539-A2B0E8881570', '51398AA0-9388-4596-AC24-594A512CC0B5', 'D091A7F4-A73A-4CCE-920E-0AB7D4C36536', N'Partial', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 5, 6, '2025-07-16 06:18:00', '2025-07-15 12:00:00'),
('E77EB0C8-24B0-422A-9480-344F7CAA859B', '51398AA0-9388-4596-AC24-594A512CC0B5', 'D091A7F4-A73A-4CCE-920E-0AB7D4C36536', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 5, 9, '2025-07-22 07:15:00', '2025-07-21 12:00:00'),
('E9619EC4-B056-4D77-A955-B3D07EFFE3E4', '51398AA0-9388-4596-AC24-594A512CC0B5', 'D091A7F4-A73A-4CCE-920E-0AB7D4C36536', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 6, 10, '2025-07-29 00:24:00', '2025-07-28 12:00:00'),
('F6F69D50-3D97-405B-A19C-37C911A50302', '51398AA0-9388-4596-AC24-594A512CC0B5', 'D091A7F4-A73A-4CCE-920E-0AB7D4C36536', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 5, 7, '2025-08-04 01:15:00', '2025-08-03 12:00:00'),
('988ECB69-57A8-4A0A-80E8-5AC6D4C91DEB', '51398AA0-9388-4596-AC24-594A512CC0B5', 'D091A7F4-A73A-4CCE-920E-0AB7D4C36536', N'Partial', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 6, 7, '2025-08-09 21:27:00', '2025-08-09 12:00:00'),
('741AC39F-6843-4D0E-9126-FB867189423A', '51398AA0-9388-4596-AC24-594A512CC0B5', 'D091A7F4-A73A-4CCE-920E-0AB7D4C36536', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 5, 9, '2025-08-16 02:49:00', '2025-08-15 12:00:00'),
('29B8712A-00EC-4E14-BF34-BFA99871942E', '51398AA0-9388-4596-AC24-594A512CC0B5', 'D091A7F4-A73A-4CCE-920E-0AB7D4C36536', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 5, 9, '2025-08-22 06:54:00', '2025-08-21 12:00:00'),
('9F4C6198-C618-46D4-96E5-AD58F47894BC', '9E6CFF62-FA31-4620-962B-F57F9BE77344', 'D091A7F4-A73A-4CCE-920E-0AB7D4C36536', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 3, 6, '2025-07-07 22:58:00', '2025-07-07 12:00:00'),
('A8EE99CF-8329-4738-87DA-1D67639EC9DD', '9E6CFF62-FA31-4620-962B-F57F9BE77344', 'D091A7F4-A73A-4CCE-920E-0AB7D4C36536', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 3, 7, '2025-07-13 21:01:00', '2025-07-13 12:00:00'),
('AF91ACFA-A168-4DCC-B6F0-626C3D507EAC', '9E6CFF62-FA31-4620-962B-F57F9BE77344', 'D091A7F4-A73A-4CCE-920E-0AB7D4C36536', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 3, 7, '2025-07-19 21:30:00', '2025-07-19 12:00:00'),
('D7745E9D-5F44-4B68-A373-A741F6FD5384', '9E6CFF62-FA31-4620-962B-F57F9BE77344', 'D091A7F4-A73A-4CCE-920E-0AB7D4C36536', N'Partial', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 5, 6, '2025-07-25 23:03:00', '2025-07-25 12:00:00'),
('A9E64E79-228A-4F65-AE1C-40658A65B231', '9E6CFF62-FA31-4620-962B-F57F9BE77344', 'D091A7F4-A73A-4CCE-920E-0AB7D4C36536', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 6, 9, '2025-08-01 09:42:00', '2025-07-31 12:00:00'),
('BD4AD939-8B54-45A4-B93F-E4054C66726C', '9E6CFF62-FA31-4620-962B-F57F9BE77344', 'D091A7F4-A73A-4CCE-920E-0AB7D4C36536', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 6, 10, '2025-08-06 06:29:00', '2025-08-05 12:00:00'),
('D258B217-A7D5-49DD-8236-89C9F8C6F857', '9E6CFF62-FA31-4620-962B-F57F9BE77344', 'D091A7F4-A73A-4CCE-920E-0AB7D4C36536', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 6, 8, '2025-08-12 00:07:00', '2025-08-11 12:00:00'),
('72685937-0E6F-4013-9965-03DD7354A70B', '9E6CFF62-FA31-4620-962B-F57F9BE77344', 'D091A7F4-A73A-4CCE-920E-0AB7D4C36536', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 7, 9, '2025-08-18 05:47:00', '2025-08-17 12:00:00'),
('FC4D8488-2649-4937-9729-61648FD0645F', '9E6CFF62-FA31-4620-962B-F57F9BE77344', 'D091A7F4-A73A-4CCE-920E-0AB7D4C36536', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 6, 8, '2025-08-23 22:01:00', '2025-08-23 12:00:00'),
('1169F903-8641-4888-A940-152CB94598F9', '776FDA27-E3CA-42DE-9CF4-7850A7F38DDE', 'D091A7F4-A73A-4CCE-920E-0AB7D4C36536', N'Partial', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 3, 3, '2025-07-06 05:53:00', '2025-07-05 12:00:00'),
('C5D91692-61A4-4168-97C4-F4883A91834A', '776FDA27-E3CA-42DE-9CF4-7850A7F38DDE', 'D091A7F4-A73A-4CCE-920E-0AB7D4C36536', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 3, 6, '2025-07-13 03:30:00', '2025-07-12 12:00:00'),
('5F7E1950-1E5D-47CE-A5E0-53BCABA88B16', '776FDA27-E3CA-42DE-9CF4-7850A7F38DDE', 'D091A7F4-A73A-4CCE-920E-0AB7D4C36536', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 5, 8, '2025-07-20 09:49:00', '2025-07-19 12:00:00'),
('C4EF08A3-BF0F-4793-A313-55ED27886804', '776FDA27-E3CA-42DE-9CF4-7850A7F38DDE', 'D091A7F4-A73A-4CCE-920E-0AB7D4C36536', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 5, 9, '2025-07-26 22:44:00', '2025-07-26 12:00:00'),
('C426999B-DFE3-4320-B222-B2DBC5726879', '776FDA27-E3CA-42DE-9CF4-7850A7F38DDE', 'D091A7F4-A73A-4CCE-920E-0AB7D4C36536', N'Partial', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 6, 8, '2025-08-04 01:22:00', '2025-08-03 12:00:00'),
('D6AE526E-D923-4CEC-9E7A-9C5149970D61', '776FDA27-E3CA-42DE-9CF4-7850A7F38DDE', 'D091A7F4-A73A-4CCE-920E-0AB7D4C36536', N'Skipped', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 4, 3, '2025-08-11 05:07:00', '2025-08-10 12:00:00'),
('5362D45E-A08B-4056-8411-CBEBF68532F9', '776FDA27-E3CA-42DE-9CF4-7850A7F38DDE', 'D091A7F4-A73A-4CCE-920E-0AB7D4C36536', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 5, 8, '2025-08-18 07:16:00', '2025-08-17 12:00:00'),
('B5F822F9-489E-4F4A-AA09-1243BCEA4F07', '776FDA27-E3CA-42DE-9CF4-7850A7F38DDE', 'D091A7F4-A73A-4CCE-920E-0AB7D4C36536', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 6, 9, '2025-08-24 22:03:00', '2025-08-24 12:00:00'),
('F39CB1AF-67D5-4014-B410-A1DBC4F1BCB3', '6F63E5BD-FEAE-4C11-9A77-37E1E9260F06', '3A5350AB-011B-41CA-A9F0-A7B2289AE70C', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 2, 5, '2025-12-05 07:15:00', '2025-12-04 12:00:00'),
('763BE86F-FE8D-4FC8-A361-504DA822EB3E', '6F63E5BD-FEAE-4C11-9A77-37E1E9260F06', '3A5350AB-011B-41CA-A9F0-A7B2289AE70C', N'Partial', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 2, 4, '2025-12-16 03:06:00', '2025-12-15 12:00:00'),
('53E519EE-1300-401A-A0EC-2B32B9CE624F', '6F63E5BD-FEAE-4C11-9A77-37E1E9260F06', '3A5350AB-011B-41CA-A9F0-A7B2289AE70C', N'Skipped', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 5, 6, '2025-12-27 08:16:00', '2025-12-26 12:00:00'),
('F4A7D8D4-69CD-4F6E-B5EF-3C6B214B9232', '6F63E5BD-FEAE-4C11-9A77-37E1E9260F06', '3A5350AB-011B-41CA-A9F0-A7B2289AE70C', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 3, 6, '2026-01-07 03:32:00', '2026-01-06 12:00:00'),
('7574DD02-34C2-414E-A9C5-3677B7153E02', '6F63E5BD-FEAE-4C11-9A77-37E1E9260F06', '3A5350AB-011B-41CA-A9F0-A7B2289AE70C', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 4, 8, '2026-01-18 08:02:00', '2026-01-17 12:00:00'),
('95BA7BDD-70C3-4F35-90DE-2B72B09250E6', '6F63E5BD-FEAE-4C11-9A77-37E1E9260F06', '3A5350AB-011B-41CA-A9F0-A7B2289AE70C', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 5, 8, '2026-01-29 07:28:00', '2026-01-28 12:00:00'),
('E1A04470-C957-4B8B-8C6A-F925DE11E664', '6F63E5BD-FEAE-4C11-9A77-37E1E9260F06', '3A5350AB-011B-41CA-A9F0-A7B2289AE70C', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 7, 9, '2026-02-08 23:09:00', '2026-02-08 12:00:00'),
('EB3814DC-7DE9-4416-9C6D-D9D1A066E9D0', '6F63E5BD-FEAE-4C11-9A77-37E1E9260F06', '3A5350AB-011B-41CA-A9F0-A7B2289AE70C', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 6, 9, '2026-02-20 01:00:00', '2026-02-19 12:00:00'),
('8E3352D3-7AC2-47CC-A6EC-A41C69B22BBA', '4AE87BEB-C39B-488F-9943-C5C71640BEB2', '3A5350AB-011B-41CA-A9F0-A7B2289AE70C', N'Partial', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 2, 3, '2025-11-30 06:27:00', '2025-11-29 12:00:00'),
('760B935A-882C-4A8C-9DA1-B4E60314A174', '4AE87BEB-C39B-488F-9943-C5C71640BEB2', '3A5350AB-011B-41CA-A9F0-A7B2289AE70C', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 4, 7, '2025-12-09 04:16:00', '2025-12-08 12:00:00'),
('23BDD588-89B1-402F-9E19-AD116B4797EA', '4AE87BEB-C39B-488F-9943-C5C71640BEB2', '3A5350AB-011B-41CA-A9F0-A7B2289AE70C', N'Partial', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 4, 5, '2025-12-19 00:56:00', '2025-12-18 12:00:00'),
('C8C254E0-6CFF-4CA6-AD11-38E47134C577', '4AE87BEB-C39B-488F-9943-C5C71640BEB2', '3A5350AB-011B-41CA-A9F0-A7B2289AE70C', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 4, 6, '2025-12-27 23:35:00', '2025-12-27 12:00:00'),
('8FC4D0A4-2D66-4A8C-91F4-C43624731302', '4AE87BEB-C39B-488F-9943-C5C71640BEB2', '3A5350AB-011B-41CA-A9F0-A7B2289AE70C', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 5, 7, '2026-01-06 08:24:00', '2026-01-05 12:00:00'),
('64FC2FF4-BB1B-44B6-90ED-49A5DE7051F7', '4AE87BEB-C39B-488F-9943-C5C71640BEB2', '3A5350AB-011B-41CA-A9F0-A7B2289AE70C', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 4, 7, '2026-01-14 22:45:00', '2026-01-14 12:00:00'),
('8141DCB9-F164-4D4B-95EC-4F81C41B5B52', '4AE87BEB-C39B-488F-9943-C5C71640BEB2', '3A5350AB-011B-41CA-A9F0-A7B2289AE70C', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 5, 9, '2026-01-24 23:07:00', '2026-01-24 12:00:00'),
('1D1BDB0A-ADA0-4107-A7B4-68DC36AFBF92', '4AE87BEB-C39B-488F-9943-C5C71640BEB2', '3A5350AB-011B-41CA-A9F0-A7B2289AE70C', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 6, 8, '2026-02-03 03:14:00', '2026-02-02 12:00:00'),
('1D4C2498-603C-42BD-9E62-8FA0B2B0A8FE', '4AE87BEB-C39B-488F-9943-C5C71640BEB2', '3A5350AB-011B-41CA-A9F0-A7B2289AE70C', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 6, 8, '2026-02-12 05:13:00', '2026-02-11 12:00:00'),
('9C41997F-A82F-4D67-8268-3CCAAF1C8DED', '7BA7486A-A9E0-444A-9EBC-556A4ACFE378', '3A5350AB-011B-41CA-A9F0-A7B2289AE70C', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 3, 7, '2025-12-07 09:40:00', '2025-12-06 12:00:00'),
('B8B5F35A-B6F2-40F6-8F6B-B55175B6C547', '7BA7486A-A9E0-444A-9EBC-556A4ACFE378', '3A5350AB-011B-41CA-A9F0-A7B2289AE70C', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 3, 6, '2025-12-29 22:36:00', '2025-12-29 12:00:00'),
('0D9D33F9-4E81-459F-9008-B161E83E89B0', '7BA7486A-A9E0-444A-9EBC-556A4ACFE378', '3A5350AB-011B-41CA-A9F0-A7B2289AE70C', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 5, 9, '2026-01-21 01:00:00', '2026-01-20 12:00:00'),
('89EA38A9-4CD2-4C42-B1A5-C8D6DC32CBBB', '7BA7486A-A9E0-444A-9EBC-556A4ACFE378', '3A5350AB-011B-41CA-A9F0-A7B2289AE70C', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 6, 8, '2026-02-13 08:15:00', '2026-02-12 12:00:00'),
('F0DF93D1-3691-4EB7-B6AC-1CDCD451100B', '75927909-0E87-4F6B-99A2-4CC9C9E27D6A', '3A5350AB-011B-41CA-A9F0-A7B2289AE70C', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 2, 5, '2025-12-01 08:05:00', '2025-11-30 12:00:00'),
('785461F8-30FA-4FC6-BE3E-13FAF12BF9E1', '75927909-0E87-4F6B-99A2-4CC9C9E27D6A', '3A5350AB-011B-41CA-A9F0-A7B2289AE70C', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 4, 6, '2025-12-10 04:08:00', '2025-12-09 12:00:00'),
('721BDA4B-4998-4867-BF44-EF63F1DE3569', '75927909-0E87-4F6B-99A2-4CC9C9E27D6A', '3A5350AB-011B-41CA-A9F0-A7B2289AE70C', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 6, 8, '2025-12-20 06:00:00', '2025-12-19 12:00:00'),
('D4C1D977-F7EE-4AAF-A3B0-2AAEA046E11E', '75927909-0E87-4F6B-99A2-4CC9C9E27D6A', '3A5350AB-011B-41CA-A9F0-A7B2289AE70C', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 7, 10, '2025-12-28 22:49:00', '2025-12-28 12:00:00'),
('1F63FCF0-2F37-44E9-B484-0088538A73F3', 'F0BB3BDF-92E5-400C-AEF7-EF4612509409', 'E07477FB-C7F6-4846-A2AB-0ED1F6DCB7AC', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 4, 7, '2025-09-27 07:07:00', '2025-09-26 12:00:00'),
('EA0E9A98-3E47-4F34-849A-7F394330C9FC', 'F0BB3BDF-92E5-400C-AEF7-EF4612509409', 'E07477FB-C7F6-4846-A2AB-0ED1F6DCB7AC', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 4, 6, '2025-10-17 08:16:00', '2025-10-16 12:00:00'),
('B847BD74-0002-4B98-BD59-78ADDE8F29BD', 'F0BB3BDF-92E5-400C-AEF7-EF4612509409', 'E07477FB-C7F6-4846-A2AB-0ED1F6DCB7AC', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 4, 7, '2025-11-07 02:37:00', '2025-11-06 12:00:00'),
('D0AFE90B-343F-4E9F-AB71-D39103069DF6', 'F0BB3BDF-92E5-400C-AEF7-EF4612509409', 'E07477FB-C7F6-4846-A2AB-0ED1F6DCB7AC', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 5, 7, '2025-11-27 02:58:00', '2025-11-26 12:00:00'),
('F06C147D-F76A-4A23-A0E4-E4803769A7E7', '847E694C-092C-40CA-BC58-C531586F1F5E', 'E07477FB-C7F6-4846-A2AB-0ED1F6DCB7AC', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 3, 5, '2025-09-30 21:02:00', '2025-09-30 12:00:00'),
('98CBEBE2-49EE-4426-AB14-BCF0B2807873', '847E694C-092C-40CA-BC58-C531586F1F5E', 'E07477FB-C7F6-4846-A2AB-0ED1F6DCB7AC', N'Partial', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 5, 6, '2025-10-12 07:51:00', '2025-10-11 12:00:00'),
('DBC2FFC5-30FE-46EB-A564-A1661972054F', '847E694C-092C-40CA-BC58-C531586F1F5E', 'E07477FB-C7F6-4846-A2AB-0ED1F6DCB7AC', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 4, 8, '2025-10-23 02:51:00', '2025-10-22 12:00:00'),
('9F0C426E-BFC0-4B64-B4A1-723892E42B73', '847E694C-092C-40CA-BC58-C531586F1F5E', 'E07477FB-C7F6-4846-A2AB-0ED1F6DCB7AC', N'Skipped', N'شعرت بهدوء أكبر بعد التمرين.', 4, 4, '2025-11-02 01:44:00', '2025-11-01 12:00:00'),
('35AC5C9C-251E-442C-AD52-DAC42E088E95', '847E694C-092C-40CA-BC58-C531586F1F5E', 'E07477FB-C7F6-4846-A2AB-0ED1F6DCB7AC', N'Partial', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 7, 8, '2025-11-13 09:35:00', '2025-11-12 12:00:00'),
('92A9440A-E119-454B-87B0-207211C69FDA', 'B152A7BF-EE40-42DE-BE98-9F9845D9035E', 'E07477FB-C7F6-4846-A2AB-0ED1F6DCB7AC', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 2, 6, '2025-10-04 03:24:00', '2025-10-03 12:00:00'),
('83CA7F46-A3A8-4F41-B61E-32B3FECD2EC8', 'B152A7BF-EE40-42DE-BE98-9F9845D9035E', 'E07477FB-C7F6-4846-A2AB-0ED1F6DCB7AC', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 5, 9, '2025-10-12 02:30:00', '2025-10-11 12:00:00'),
('6BD5AC4C-E0F0-4878-B495-47BD2DE478AE', 'B152A7BF-EE40-42DE-BE98-9F9845D9035E', 'E07477FB-C7F6-4846-A2AB-0ED1F6DCB7AC', N'Skipped', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 4, 4, '2025-10-18 20:22:00', '2025-10-18 12:00:00'),
('4F48D71D-CFC5-4943-957A-B0F949A2198A', 'B152A7BF-EE40-42DE-BE98-9F9845D9035E', 'E07477FB-C7F6-4846-A2AB-0ED1F6DCB7AC', N'Skipped', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 5, 6, '2025-10-27 09:42:00', '2025-10-26 12:00:00'),
('4ECBD6AE-028F-43C8-AFC1-13B8434A1A54', 'B152A7BF-EE40-42DE-BE98-9F9845D9035E', 'E07477FB-C7F6-4846-A2AB-0ED1F6DCB7AC', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 7, 10, '2025-11-03 07:38:00', '2025-11-02 12:00:00'),
('435899CC-77CF-40EB-B1C5-DA0A1EBAF038', 'E853FBC7-D555-4D8D-B854-4D482EA2EB80', 'E07477FB-C7F6-4846-A2AB-0ED1F6DCB7AC', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 2, 5, '2025-09-28 02:02:00', '2025-09-27 12:00:00'),
('5A1251DE-E340-42FC-B5EC-561A42276455', 'E853FBC7-D555-4D8D-B854-4D482EA2EB80', 'E07477FB-C7F6-4846-A2AB-0ED1F6DCB7AC', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 4, 7, '2025-10-08 08:51:00', '2025-10-07 12:00:00'),
('7DB84DC5-97C3-4458-86D2-185327BC763F', 'E853FBC7-D555-4D8D-B854-4D482EA2EB80', 'E07477FB-C7F6-4846-A2AB-0ED1F6DCB7AC', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 3, 5, '2025-10-18 00:59:00', '2025-10-17 12:00:00'),
('F1833981-727A-4EBE-BD32-91BB2B80C03B', 'E853FBC7-D555-4D8D-B854-4D482EA2EB80', 'E07477FB-C7F6-4846-A2AB-0ED1F6DCB7AC', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 6, 10, '2025-10-26 21:59:00', '2025-10-26 12:00:00'),
('4BD358B9-E3B5-4B55-9CB8-CDD1457C080B', 'E853FBC7-D555-4D8D-B854-4D482EA2EB80', 'E07477FB-C7F6-4846-A2AB-0ED1F6DCB7AC', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 6, 8, '2025-11-05 23:08:00', '2025-11-05 12:00:00'),
('AB5751FE-C7BE-414A-9B14-553876F87AEA', 'E853FBC7-D555-4D8D-B854-4D482EA2EB80', 'E07477FB-C7F6-4846-A2AB-0ED1F6DCB7AC', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 5, 7, '2025-11-16 05:00:00', '2025-11-15 12:00:00'),
('93DBEA45-1872-4C4D-9668-86D863910DF0', '5AB551C9-3894-46AA-A875-DBF379A5ADC1', 'DC19C765-09E1-4B24-808B-8EFC97E63589', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 2, 6, '2026-03-16 21:19:00', '2026-03-16 12:00:00'),
('9D2BE9C6-BF11-4366-95C3-A88D444960D1', '5AB551C9-3894-46AA-A875-DBF379A5ADC1', 'DC19C765-09E1-4B24-808B-8EFC97E63589', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 3, 5, '2026-03-25 02:01:00', '2026-03-24 12:00:00'),
('22668BAB-A443-4DF9-B03A-0EEF2DFE438B', '5AB551C9-3894-46AA-A875-DBF379A5ADC1', 'DC19C765-09E1-4B24-808B-8EFC97E63589', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 3, 7, '2026-04-02 01:09:00', '2026-04-01 12:00:00'),
('D93F0F46-4209-4047-9E9C-9AC6EF7FB2CB', '5AB551C9-3894-46AA-A875-DBF379A5ADC1', 'DC19C765-09E1-4B24-808B-8EFC97E63589', N'Partial', N'شعرت بهدوء أكبر بعد التمرين.', 5, 5, '2026-04-10 03:09:00', '2026-04-09 12:00:00'),
('66CA075F-AEE6-44C4-A0D1-5759021072AC', '5AB551C9-3894-46AA-A875-DBF379A5ADC1', 'DC19C765-09E1-4B24-808B-8EFC97E63589', N'Partial', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 6, 8, '2026-04-18 20:35:00', '2026-04-18 12:00:00'),
('2F1A6A9D-8749-4B89-A502-2CADD7BB4A9C', '5AB551C9-3894-46AA-A875-DBF379A5ADC1', 'DC19C765-09E1-4B24-808B-8EFC97E63589', N'Partial', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 5, 6, '2026-04-27 02:56:00', '2026-04-26 12:00:00'),
('93517998-A75A-4176-8477-906D368B9972', '5AB551C9-3894-46AA-A875-DBF379A5ADC1', 'DC19C765-09E1-4B24-808B-8EFC97E63589', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 7, 10, '2026-05-05 00:10:00', '2026-05-04 12:00:00'),
('44CDC5F0-BADC-438B-9AA4-ACAE9871FFCE', '5AB551C9-3894-46AA-A875-DBF379A5ADC1', 'DC19C765-09E1-4B24-808B-8EFC97E63589', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 6, 9, '2026-05-13 07:08:00', '2026-05-12 12:00:00'),
('32B19B7C-4B10-4DC5-B1A3-A64824AD3F20', '128FA5D6-04D3-40C1-BF02-18B517FFCAB0', 'DC19C765-09E1-4B24-808B-8EFC97E63589', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 3, 5, '2026-03-15 09:32:00', '2026-03-14 12:00:00'),
('4CFDD7F3-1A80-41DD-ACF5-E053BFCD9A96', '128FA5D6-04D3-40C1-BF02-18B517FFCAB0', 'DC19C765-09E1-4B24-808B-8EFC97E63589', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 5, 8, '2026-03-23 07:59:00', '2026-03-22 12:00:00'),
('351F8167-5D3F-45DC-9C4C-29F1A851CE38', '128FA5D6-04D3-40C1-BF02-18B517FFCAB0', 'DC19C765-09E1-4B24-808B-8EFC97E63589', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 5, 7, '2026-03-29 21:04:00', '2026-03-29 12:00:00'),
('6ADF9298-A92B-4FAA-8365-2500A3A4A261', '128FA5D6-04D3-40C1-BF02-18B517FFCAB0', 'DC19C765-09E1-4B24-808B-8EFC97E63589', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 4, 7, '2026-04-07 05:19:00', '2026-04-06 12:00:00'),
('62866030-E4C3-4435-BADF-04E74D3E0D4A', '128FA5D6-04D3-40C1-BF02-18B517FFCAB0', 'DC19C765-09E1-4B24-808B-8EFC97E63589', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 5, 9, '2026-04-13 21:04:00', '2026-04-13 12:00:00'),
('0BEBBE07-9942-4140-97C0-EB541DC24B29', '128FA5D6-04D3-40C1-BF02-18B517FFCAB0', 'DC19C765-09E1-4B24-808B-8EFC97E63589', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 7, 10, '2026-04-22 06:45:00', '2026-04-21 12:00:00'),
('0EE248E4-44E8-43ED-9B23-B3692E45FDF8', '128FA5D6-04D3-40C1-BF02-18B517FFCAB0', 'DC19C765-09E1-4B24-808B-8EFC97E63589', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 5, 7, '2026-04-28 22:39:00', '2026-04-28 12:00:00'),
('0F9D388A-55DC-4C8C-A4ED-2F81047E7E21', '47CE33C0-0FF1-4479-83B0-69C48AC1202F', '8F8DC8EB-DCBE-44D2-B913-73C2CE8E75B3', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 2, 5, '2026-01-30 23:37:00', '2026-01-30 12:00:00'),
('A3970433-E3F4-49D9-A4B0-A7BF1A167241', '47CE33C0-0FF1-4479-83B0-69C48AC1202F', '8F8DC8EB-DCBE-44D2-B913-73C2CE8E75B3', N'Partial', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 4, 5, '2026-02-12 08:20:00', '2026-02-11 12:00:00'),
('6834FFCD-32E5-4224-A6C2-3B131AB0DACE', '47CE33C0-0FF1-4479-83B0-69C48AC1202F', '8F8DC8EB-DCBE-44D2-B913-73C2CE8E75B3', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 4, 7, '2026-02-23 20:28:00', '2026-02-23 12:00:00'),
('EDE6F326-D746-42F9-B900-23D07BF6B091', '47CE33C0-0FF1-4479-83B0-69C48AC1202F', '8F8DC8EB-DCBE-44D2-B913-73C2CE8E75B3', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 6, 9, '2026-03-07 21:47:00', '2026-03-07 12:00:00'),
('4E05139F-B9EF-420F-B8C1-7D2E4EF12FD6', '47CE33C0-0FF1-4479-83B0-69C48AC1202F', '8F8DC8EB-DCBE-44D2-B913-73C2CE8E75B3', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 4, 7, '2026-03-20 04:01:00', '2026-03-19 12:00:00'),
('70334C95-8F7A-4CBE-9872-5EDE86205BB9', '47CE33C0-0FF1-4479-83B0-69C48AC1202F', '8F8DC8EB-DCBE-44D2-B913-73C2CE8E75B3', N'Partial', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 7, 9, '2026-03-31 22:44:00', '2026-03-31 12:00:00'),
('13491590-104C-466F-B9F6-2422F66ADEB8', '47CE33C0-0FF1-4479-83B0-69C48AC1202F', '8F8DC8EB-DCBE-44D2-B913-73C2CE8E75B3', N'Partial', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 7, 8, '2026-04-13 09:57:00', '2026-04-12 12:00:00'),
('7E7E4017-1BA2-49A7-9A7E-D11E3F05E69B', '9257536E-95FE-4FBD-BC54-E10FFA2CED62', '8F8DC8EB-DCBE-44D2-B913-73C2CE8E75B3', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 2, 6, '2026-01-27 23:36:00', '2026-01-27 12:00:00'),
('AAE0A74D-64A3-44CB-8E60-857C3E0CF443', '9257536E-95FE-4FBD-BC54-E10FFA2CED62', '8F8DC8EB-DCBE-44D2-B913-73C2CE8E75B3', N'Skipped', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 3, 2, '2026-02-08 02:05:00', '2026-02-07 12:00:00'),
('186E1A5D-8857-4742-AED4-9DD09B1D3EEC', '9257536E-95FE-4FBD-BC54-E10FFA2CED62', '8F8DC8EB-DCBE-44D2-B913-73C2CE8E75B3', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 4, 6, '2026-02-18 00:57:00', '2026-02-17 12:00:00'),
('7A7D6882-136B-455B-B58E-FB2E10EF3734', '9257536E-95FE-4FBD-BC54-E10FFA2CED62', '8F8DC8EB-DCBE-44D2-B913-73C2CE8E75B3', N'Partial', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 5, 5, '2026-03-01 05:22:00', '2026-02-28 12:00:00'),
('57D90692-5D63-4577-A3B9-637DA92892EC', '9257536E-95FE-4FBD-BC54-E10FFA2CED62', '8F8DC8EB-DCBE-44D2-B913-73C2CE8E75B3', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 4, 7, '2026-03-12 04:46:00', '2026-03-11 12:00:00'),
('03A6C41D-7198-49F2-A254-09E989B25E38', '9257536E-95FE-4FBD-BC54-E10FFA2CED62', '8F8DC8EB-DCBE-44D2-B913-73C2CE8E75B3', N'Skipped', N'شعرت بهدوء أكبر بعد التمرين.', 7, 7, '2026-03-22 02:31:00', '2026-03-21 12:00:00'),
('1AE8AD2A-FB1C-4012-AE7A-E18F2879CC08', '9257536E-95FE-4FBD-BC54-E10FFA2CED62', '8F8DC8EB-DCBE-44D2-B913-73C2CE8E75B3', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 7, 9, '2026-04-02 04:35:00', '2026-04-01 12:00:00'),
('DEE013DF-FE9A-485A-838A-F48251D6D71C', 'A0D7F743-526A-4B8C-841D-22A6BCCF732F', '8F8DC8EB-DCBE-44D2-B913-73C2CE8E75B3', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 4, 6, '2026-01-29 04:11:00', '2026-01-28 12:00:00'),
('6F05457B-BDA0-4A51-BA1F-627A7CD6C8C7', 'A0D7F743-526A-4B8C-841D-22A6BCCF732F', '8F8DC8EB-DCBE-44D2-B913-73C2CE8E75B3', N'Partial', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 4, 6, '2026-02-04 23:27:00', '2026-02-04 12:00:00'),
('25FEB121-613B-48A3-9DBF-7089DD61C0BF', 'A0D7F743-526A-4B8C-841D-22A6BCCF732F', '8F8DC8EB-DCBE-44D2-B913-73C2CE8E75B3', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 4, 6, '2026-02-12 21:34:00', '2026-02-12 12:00:00'),
('5F433CC8-81BF-4CF8-B5A0-D01DBE572CEC', 'A0D7F743-526A-4B8C-841D-22A6BCCF732F', '8F8DC8EB-DCBE-44D2-B913-73C2CE8E75B3', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 4, 8, '2026-02-20 05:34:00', '2026-02-19 12:00:00'),
('C7524D85-C499-4D57-96AA-89C27944C676', 'A0D7F743-526A-4B8C-841D-22A6BCCF732F', '8F8DC8EB-DCBE-44D2-B913-73C2CE8E75B3', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 4, 8, '2026-02-27 08:18:00', '2026-02-26 12:00:00'),
('3571CC0A-BD4C-41B1-8408-9D79D3B77963', 'A0D7F743-526A-4B8C-841D-22A6BCCF732F', '8F8DC8EB-DCBE-44D2-B913-73C2CE8E75B3', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 4, 8, '2026-03-06 07:16:00', '2026-03-05 12:00:00'),
('E655FBE8-C040-4262-983F-8195D86F2BBD', 'A0D7F743-526A-4B8C-841D-22A6BCCF732F', '8F8DC8EB-DCBE-44D2-B913-73C2CE8E75B3', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 5, 8, '2026-03-14 00:08:00', '2026-03-13 12:00:00'),
('FBDA0B22-E9FB-43A4-9371-B547459E81E7', 'A0D7F743-526A-4B8C-841D-22A6BCCF732F', '8F8DC8EB-DCBE-44D2-B913-73C2CE8E75B3', N'Partial', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 5, 7, '2026-03-21 01:20:00', '2026-03-20 12:00:00'),
('8849D13C-77CF-402A-BEE4-4C0948E6912C', '2BEA48B4-24A0-4E09-9028-458D1CE3F579', '8F8DC8EB-DCBE-44D2-B913-73C2CE8E75B3', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 3, 6, '2026-01-27 06:00:00', '2026-01-26 12:00:00'),
('1939BB52-509E-4C37-B524-5562C2C1AE7E', '2BEA48B4-24A0-4E09-9028-458D1CE3F579', '8F8DC8EB-DCBE-44D2-B913-73C2CE8E75B3', N'Skipped', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 2, 3, '2026-02-07 03:49:00', '2026-02-06 12:00:00'),
('EE68E893-919A-4858-83C7-5F0E222C9B41', '2BEA48B4-24A0-4E09-9028-458D1CE3F579', '8F8DC8EB-DCBE-44D2-B913-73C2CE8E75B3', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 4, 7, '2026-02-19 01:01:00', '2026-02-18 12:00:00'),
('24C91C55-EC18-4611-992B-65CBEDB4DDF8', '2BEA48B4-24A0-4E09-9028-458D1CE3F579', '8F8DC8EB-DCBE-44D2-B913-73C2CE8E75B3', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 3, 6, '2026-03-02 06:04:00', '2026-03-01 12:00:00'),
('C2A66929-9B67-4644-A106-11C8169AF652', '2BEA48B4-24A0-4E09-9028-458D1CE3F579', '8F8DC8EB-DCBE-44D2-B913-73C2CE8E75B3', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 6, 9, '2026-03-13 03:31:00', '2026-03-12 12:00:00'),
('B292879C-5EC0-49A3-A01D-20433DFBAD4D', '2BEA48B4-24A0-4E09-9028-458D1CE3F579', '8F8DC8EB-DCBE-44D2-B913-73C2CE8E75B3', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 6, 9, '2026-03-24 04:00:00', '2026-03-23 12:00:00'),
('E7E3760B-9D1B-4F5B-B2EE-EBE539126989', '2BEA48B4-24A0-4E09-9028-458D1CE3F579', '8F8DC8EB-DCBE-44D2-B913-73C2CE8E75B3', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 7, 9, '2026-04-05 04:58:00', '2026-04-04 12:00:00'),
('AD2B0A32-7C03-4369-9DA2-DC2FD5894F89', '2BEA48B4-24A0-4E09-9028-458D1CE3F579', '8F8DC8EB-DCBE-44D2-B913-73C2CE8E75B3', N'Skipped', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 7, 8, '2026-04-16 06:37:00', '2026-04-15 12:00:00'),
('7CEC9794-2F9D-4924-B8BF-3F6D73303B82', '95372A36-329F-4AC4-B8DF-18F96CF24ABF', 'F9DD6265-AFC8-44EC-8985-D0E90571FCA6', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 4, 6, '2026-02-12 00:18:00', '2026-02-11 12:00:00'),
('884046A8-5F38-401D-A45C-B2A5277F63A1', '95372A36-329F-4AC4-B8DF-18F96CF24ABF', 'F9DD6265-AFC8-44EC-8985-D0E90571FCA6', N'Skipped', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 4, 5, '2026-02-24 02:09:00', '2026-02-23 12:00:00'),
('41285BF8-334C-41D6-AEDD-E3A781101F68', '95372A36-329F-4AC4-B8DF-18F96CF24ABF', 'F9DD6265-AFC8-44EC-8985-D0E90571FCA6', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 3, 5, '2026-03-07 02:41:00', '2026-03-06 12:00:00'),
('1F16C789-4C41-443E-98B4-0E8967A510FB', '95372A36-329F-4AC4-B8DF-18F96CF24ABF', 'F9DD6265-AFC8-44EC-8985-D0E90571FCA6', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 4, 8, '2026-03-19 03:51:00', '2026-03-18 12:00:00'),
('4E18AD41-3B3C-4AAD-A711-41BF03ED5905', '95372A36-329F-4AC4-B8DF-18F96CF24ABF', 'F9DD6265-AFC8-44EC-8985-D0E90571FCA6', N'Skipped', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 6, 6, '2026-03-30 09:04:00', '2026-03-29 12:00:00'),
('C60F1047-6FB4-4AA7-BEC0-C91523601D7A', '95372A36-329F-4AC4-B8DF-18F96CF24ABF', 'F9DD6265-AFC8-44EC-8985-D0E90571FCA6', N'Skipped', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 5, 6, '2026-04-11 04:10:00', '2026-04-10 12:00:00'),
('734EB9C7-8B93-4391-B035-A5EE8FD3AE1A', '95372A36-329F-4AC4-B8DF-18F96CF24ABF', 'F9DD6265-AFC8-44EC-8985-D0E90571FCA6', N'Skipped', N'شعرت بهدوء أكبر بعد التمرين.', 5, 4, '2026-04-22 03:13:00', '2026-04-21 12:00:00'),
('C7D1C504-AF8F-432D-9624-D05D064094F3', '46D2D003-9146-49A8-89F1-852D99BE15F0', 'F9DD6265-AFC8-44EC-8985-D0E90571FCA6', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 2, 5, '2026-02-06 02:43:00', '2026-02-05 12:00:00'),
('4D8B1FF1-C4BF-4D3F-A792-2500467AE539', '46D2D003-9146-49A8-89F1-852D99BE15F0', 'F9DD6265-AFC8-44EC-8985-D0E90571FCA6', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 4, 8, '2026-02-12 08:19:00', '2026-02-11 12:00:00'),
('F20C1F0D-7AC4-4343-8A9B-0C81439832E9', '46D2D003-9146-49A8-89F1-852D99BE15F0', 'F9DD6265-AFC8-44EC-8985-D0E90571FCA6', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 5, 8, '2026-02-19 07:33:00', '2026-02-18 12:00:00'),
('F1F14DCD-2663-4CDB-9056-3029011DC88B', '46D2D003-9146-49A8-89F1-852D99BE15F0', 'F9DD6265-AFC8-44EC-8985-D0E90571FCA6', N'Partial', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 5, 5, '2026-02-24 23:47:00', '2026-02-24 12:00:00'),
('EBD3AD10-B55D-488E-BCE9-0C427C5C19C7', '46D2D003-9146-49A8-89F1-852D99BE15F0', 'F9DD6265-AFC8-44EC-8985-D0E90571FCA6', N'Partial', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 5, 6, '2026-03-04 05:09:00', '2026-03-03 12:00:00'),
('678382B2-F6A5-458A-8552-3351A19D0C16', '46D2D003-9146-49A8-89F1-852D99BE15F0', 'F9DD6265-AFC8-44EC-8985-D0E90571FCA6', N'Skipped', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 6, 5, '2026-03-10 00:09:00', '2026-03-09 12:00:00'),
('A7443924-6806-4A72-A790-435F0B5488C9', '2B5DEB16-BBAE-4FB1-BF25-F4B2DBC27F75', 'F9DD6265-AFC8-44EC-8985-D0E90571FCA6', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 2, 4, '2026-02-10 01:11:00', '2026-02-09 12:00:00'),
('FA4C4309-D0D1-40B2-A779-6F8FF496307A', '2B5DEB16-BBAE-4FB1-BF25-F4B2DBC27F75', 'F9DD6265-AFC8-44EC-8985-D0E90571FCA6', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 5, 9, '2026-02-19 01:23:00', '2026-02-18 12:00:00'),
('59121635-A758-4D11-89D2-4BA886DF3C51', '2B5DEB16-BBAE-4FB1-BF25-F4B2DBC27F75', 'F9DD6265-AFC8-44EC-8985-D0E90571FCA6', N'Skipped', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 5, 4, '2026-02-27 07:17:00', '2026-02-26 12:00:00'),
('66000611-92F3-4A7C-B93A-B3C7B1E59CCB', '2B5DEB16-BBAE-4FB1-BF25-F4B2DBC27F75', 'F9DD6265-AFC8-44EC-8985-D0E90571FCA6', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 6, 8, '2026-03-08 08:23:00', '2026-03-07 12:00:00'),
('6FFF34D4-B112-4767-AAD7-B633A16D7CF2', '2B5DEB16-BBAE-4FB1-BF25-F4B2DBC27F75', 'F9DD6265-AFC8-44EC-8985-D0E90571FCA6', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 7, 10, '2026-03-16 06:18:00', '2026-03-15 12:00:00'),
('2878E61D-F224-43D2-B2D1-A0CD39ECA730', 'BC5ECDCD-510E-4F63-A4C8-035A68C1ADFB', 'F9DD6265-AFC8-44EC-8985-D0E90571FCA6', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 4, 7, '2026-02-08 22:52:00', '2026-02-08 12:00:00'),
('1D46A0B1-5355-49E3-977A-16F02931075E', 'BC5ECDCD-510E-4F63-A4C8-035A68C1ADFB', 'F9DD6265-AFC8-44EC-8985-D0E90571FCA6', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 4, 8, '2026-02-15 06:01:00', '2026-02-14 12:00:00'),
('BF3FDFF4-C2CB-4E07-B4FA-3502427FAEBF', 'BC5ECDCD-510E-4F63-A4C8-035A68C1ADFB', 'F9DD6265-AFC8-44EC-8985-D0E90571FCA6', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 3, 5, '2026-02-21 09:28:00', '2026-02-20 12:00:00'),
('E2F7F437-33AF-4425-A0FB-BBF91AE9AD53', 'BC5ECDCD-510E-4F63-A4C8-035A68C1ADFB', 'F9DD6265-AFC8-44EC-8985-D0E90571FCA6', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 6, 9, '2026-02-26 23:08:00', '2026-02-26 12:00:00'),
('E2E2331C-2BA7-4B14-A797-8C9D6D89BC14', 'BC5ECDCD-510E-4F63-A4C8-035A68C1ADFB', 'F9DD6265-AFC8-44EC-8985-D0E90571FCA6', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 6, 9, '2026-03-05 05:47:00', '2026-03-04 12:00:00'),
('1DF964CE-ADEE-4B2B-8565-F6D3FD806024', 'BC5ECDCD-510E-4F63-A4C8-035A68C1ADFB', 'F9DD6265-AFC8-44EC-8985-D0E90571FCA6', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 5, 7, '2026-03-11 06:05:00', '2026-03-10 12:00:00'),
('60B1D8DE-829D-4773-A886-E31C3276B21F', '901EE028-7107-4753-A19B-888138AB9BE8', 'A8BC8AD9-EC06-4493-8502-F88E2C965073', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 2, 5, '2026-06-27 05:46:00', '2026-06-26 12:00:00'),
('88DB3728-AC39-4B95-B75B-CD7414DE6543', '901EE028-7107-4753-A19B-888138AB9BE8', 'A8BC8AD9-EC06-4493-8502-F88E2C965073', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 5, 8, '2026-06-30 02:49:00', '2026-06-29 12:00:00'),
('1F54E05E-E90E-42B6-9B01-6F2E8D8EA49F', '901EE028-7107-4753-A19B-888138AB9BE8', 'A8BC8AD9-EC06-4493-8502-F88E2C965073', N'Skipped', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 4, 5, '2026-07-02 04:04:00', '2026-07-01 12:00:00'),
('6D392AE7-04A7-4857-8303-66EB4C7B16E2', '901EE028-7107-4753-A19B-888138AB9BE8', 'A8BC8AD9-EC06-4493-8502-F88E2C965073', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 5, 9, '2026-07-05 09:26:00', '2026-07-04 12:00:00'),
('6C4DA07E-4FF3-4404-8FD4-C2C580690C1D', '901EE028-7107-4753-A19B-888138AB9BE8', 'A8BC8AD9-EC06-4493-8502-F88E2C965073', N'Partial', N'شعرت بهدوء أكبر بعد التمرين.', 7, 7, '2026-07-07 02:33:00', '2026-07-06 12:00:00'),
('557D817F-FCD9-427E-A470-EE9E1F07CD9C', 'A25E0F60-CD12-48FA-A926-586A2E1EBB28', 'A8BC8AD9-EC06-4493-8502-F88E2C965073', N'Partial', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 3, 3, '2026-06-26 20:47:00', '2026-06-26 12:00:00'),
('AD2EFB93-3140-4C00-9572-5E70EFBCC1BD', 'A25E0F60-CD12-48FA-A926-586A2E1EBB28', 'A8BC8AD9-EC06-4493-8502-F88E2C965073', N'Partial', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 5, 5, '2026-06-30 09:41:00', '2026-06-29 12:00:00'),
('54279630-443D-4908-AD88-82EF3FA39F89', 'A25E0F60-CD12-48FA-A926-586A2E1EBB28', 'A8BC8AD9-EC06-4493-8502-F88E2C965073', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 4, 6, '2026-07-03 22:35:00', '2026-07-03 12:00:00'),
('EBC0C847-F660-4EB0-BB06-320FB5BC099E', 'A25E0F60-CD12-48FA-A926-586A2E1EBB28', 'A8BC8AD9-EC06-4493-8502-F88E2C965073', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 7, 10, '2026-07-06 20:50:00', '2026-07-06 12:00:00'),
('2785F5A1-0DD5-4170-AEF2-30D2A06DB753', '01986D11-3410-4C5A-8D8C-054C54975AE6', 'A8BC8AD9-EC06-4493-8502-F88E2C965073', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 2, 4, '2026-06-20 21:39:00', '2026-06-20 12:00:00'),
('835FACE2-3653-483B-BC6A-80263EF25DEE', '01986D11-3410-4C5A-8D8C-054C54975AE6', 'A8BC8AD9-EC06-4493-8502-F88E2C965073', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 4, 7, '2026-06-24 20:13:00', '2026-06-24 12:00:00'),
('05261C9A-ED88-4594-AFA3-41892EB02F7C', '01986D11-3410-4C5A-8D8C-054C54975AE6', 'A8BC8AD9-EC06-4493-8502-F88E2C965073', N'Partial', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 4, 4, '2026-06-28 21:37:00', '2026-06-28 12:00:00'),
('C04C3A29-3DC2-48E3-B3BA-7CAC09865639', '01986D11-3410-4C5A-8D8C-054C54975AE6', 'A8BC8AD9-EC06-4493-8502-F88E2C965073', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 6, 8, '2026-07-03 09:56:00', '2026-07-02 12:00:00'),
('CF94AF13-4039-4167-881C-961C7EB185F1', '01986D11-3410-4C5A-8D8C-054C54975AE6', 'A8BC8AD9-EC06-4493-8502-F88E2C965073', N'Partial', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 7, 9, '2026-07-06 23:27:00', '2026-07-06 12:00:00'),
('83892525-2AB9-499E-9094-8E345E49E1F3', 'F353A6EB-0B0E-4826-A303-B3E392F52B0F', 'A8BC8AD9-EC06-4493-8502-F88E2C965073', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 4, 6, '2026-06-25 22:43:00', '2026-06-25 12:00:00'),
('755A1560-49A6-41D1-AD76-F5D07C25BB20', 'F353A6EB-0B0E-4826-A303-B3E392F52B0F', 'A8BC8AD9-EC06-4493-8502-F88E2C965073', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 4, 8, '2026-06-28 21:02:00', '2026-06-28 12:00:00'),
('FFF71388-6295-425A-8E59-ABEAB6EADABC', 'F353A6EB-0B0E-4826-A303-B3E392F52B0F', 'A8BC8AD9-EC06-4493-8502-F88E2C965073', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 4, 7, '2026-07-01 22:58:00', '2026-07-01 12:00:00'),
('BD292685-8611-447C-8006-62537A7BB6AF', 'F353A6EB-0B0E-4826-A303-B3E392F52B0F', 'A8BC8AD9-EC06-4493-8502-F88E2C965073', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 5, 9, '2026-07-03 22:41:00', '2026-07-03 12:00:00'),
('5969A8B1-B83B-4F8A-B6E0-8FB4E062F13F', 'F353A6EB-0B0E-4826-A303-B3E392F52B0F', 'A8BC8AD9-EC06-4493-8502-F88E2C965073', N'Skipped', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 5, 6, '2026-07-07 08:38:00', '2026-07-06 12:00:00'),
('20AE6432-EBF4-418C-A18E-D4870BB96564', '2785B286-2187-4953-9BCA-C79E211D522E', '2557FC9B-7599-4EB3-9FA4-7DCD1EAE6EDA', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 4, 6, '2026-03-01 06:23:00', '2026-02-28 12:00:00'),
('F34DA949-4CD2-4B33-A913-630B79AEBBCC', '2785B286-2187-4953-9BCA-C79E211D522E', '2557FC9B-7599-4EB3-9FA4-7DCD1EAE6EDA', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 3, 5, '2026-03-12 22:31:00', '2026-03-12 12:00:00'),
('E831B5D5-DB71-4684-B767-992A5876C5B6', '2785B286-2187-4953-9BCA-C79E211D522E', '2557FC9B-7599-4EB3-9FA4-7DCD1EAE6EDA', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 5, 7, '2026-03-26 06:55:00', '2026-03-25 12:00:00'),
('847ABC89-2D8D-4B83-8082-657BE7230D28', '2785B286-2187-4953-9BCA-C79E211D522E', '2557FC9B-7599-4EB3-9FA4-7DCD1EAE6EDA', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 5, 8, '2026-04-07 00:36:00', '2026-04-06 12:00:00'),
('0CDF90D5-C4D5-4734-8D6B-D86C85B34306', 'E9E340BC-01DE-4F53-8A24-021AE42BC37D', '2557FC9B-7599-4EB3-9FA4-7DCD1EAE6EDA', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 4, 7, '2026-02-26 01:20:00', '2026-02-25 12:00:00'),
('F76B0A9C-EC08-4692-BCCE-98B0E4B208F9', 'E9E340BC-01DE-4F53-8A24-021AE42BC37D', '2557FC9B-7599-4EB3-9FA4-7DCD1EAE6EDA', N'Partial', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 3, 5, '2026-03-10 01:24:00', '2026-03-09 12:00:00'),
('1F326443-E3A2-4077-995A-88CD0A6BA408', 'E9E340BC-01DE-4F53-8A24-021AE42BC37D', '2557FC9B-7599-4EB3-9FA4-7DCD1EAE6EDA', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 5, 9, '2026-03-23 09:29:00', '2026-03-22 12:00:00'),
('C7F92733-7CA0-47A4-B912-43C766DC191B', 'E9E340BC-01DE-4F53-8A24-021AE42BC37D', '2557FC9B-7599-4EB3-9FA4-7DCD1EAE6EDA', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 6, 10, '2026-04-04 05:06:00', '2026-04-03 12:00:00'),
('A205F970-95FC-47F1-B275-6F54E5F1750E', 'E9E340BC-01DE-4F53-8A24-021AE42BC37D', '2557FC9B-7599-4EB3-9FA4-7DCD1EAE6EDA', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 7, 10, '2026-04-16 05:06:00', '2026-04-15 12:00:00'),
('D6E94C3C-1D74-417E-8375-321BAD8505DB', '32111629-8DD8-464B-B653-D738A61738DD', '2557FC9B-7599-4EB3-9FA4-7DCD1EAE6EDA', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 3, 7, '2026-03-03 06:17:00', '2026-03-02 12:00:00'),
('15356548-8309-406E-818D-447FE37BF213', '32111629-8DD8-464B-B653-D738A61738DD', '2557FC9B-7599-4EB3-9FA4-7DCD1EAE6EDA', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 4, 8, '2026-03-07 21:14:00', '2026-03-07 12:00:00'),
('B6F8AAF4-12EA-47FC-B90F-3482855B1980', '32111629-8DD8-464B-B653-D738A61738DD', '2557FC9B-7599-4EB3-9FA4-7DCD1EAE6EDA', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 4, 7, '2026-03-14 02:15:00', '2026-03-13 12:00:00'),
('BCA3AC28-CE11-41A2-8CD7-111D831514E1', '32111629-8DD8-464B-B653-D738A61738DD', '2557FC9B-7599-4EB3-9FA4-7DCD1EAE6EDA', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 5, 7, '2026-03-19 06:22:00', '2026-03-18 12:00:00'),
('5C344384-FCF5-45AC-9807-3040F47BAC1E', '32111629-8DD8-464B-B653-D738A61738DD', '2557FC9B-7599-4EB3-9FA4-7DCD1EAE6EDA', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 6, 9, '2026-03-25 07:14:00', '2026-03-24 12:00:00'),
('17766CB1-EFDB-4AA8-9349-71D64DC4840C', '32111629-8DD8-464B-B653-D738A61738DD', '2557FC9B-7599-4EB3-9FA4-7DCD1EAE6EDA', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 4, 8, '2026-03-30 04:52:00', '2026-03-29 12:00:00'),
('A679F218-35DC-4E6B-894C-EF331305DA95', '32111629-8DD8-464B-B653-D738A61738DD', '2557FC9B-7599-4EB3-9FA4-7DCD1EAE6EDA', N'Skipped', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 7, 8, '2026-04-04 23:19:00', '2026-04-04 12:00:00'),
('2D3C2800-D606-42D0-A007-F2D693F64AB8', '32111629-8DD8-464B-B653-D738A61738DD', '2557FC9B-7599-4EB3-9FA4-7DCD1EAE6EDA', N'Partial', N'شعرت بهدوء أكبر بعد التمرين.', 7, 9, '2026-04-10 06:26:00', '2026-04-09 12:00:00'),
('1E08D666-442B-406C-8BB4-C23B742CD916', '7D706C26-4E08-4553-886C-14D328984985', '2557FC9B-7599-4EB3-9FA4-7DCD1EAE6EDA', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 3, 5, '2026-02-24 20:23:00', '2026-02-24 12:00:00'),
('9B30144C-32B4-43C5-A0D8-0C317ABEA8DF', '7D706C26-4E08-4553-886C-14D328984985', '2557FC9B-7599-4EB3-9FA4-7DCD1EAE6EDA', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 4, 7, '2026-03-01 23:21:00', '2026-03-01 12:00:00'),
('BB051E6E-5CA8-406A-B8FC-0C2772B2756D', '7D706C26-4E08-4553-886C-14D328984985', '2557FC9B-7599-4EB3-9FA4-7DCD1EAE6EDA', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 3, 6, '2026-03-06 21:20:00', '2026-03-06 12:00:00'),
('6ACD13F7-7878-4C3B-9F3F-54456DBD13CE', '7D706C26-4E08-4553-886C-14D328984985', '2557FC9B-7599-4EB3-9FA4-7DCD1EAE6EDA', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 5, 9, '2026-03-12 08:18:00', '2026-03-11 12:00:00'),
('C62E7BF3-14E0-4977-966B-CC0C057A0E06', '7D706C26-4E08-4553-886C-14D328984985', '2557FC9B-7599-4EB3-9FA4-7DCD1EAE6EDA', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 6, 10, '2026-03-18 04:19:00', '2026-03-17 12:00:00'),
('B15CD0BF-CFA3-4F7D-A165-408EF3E82A1A', '7D706C26-4E08-4553-886C-14D328984985', '2557FC9B-7599-4EB3-9FA4-7DCD1EAE6EDA', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 5, 9, '2026-03-23 09:43:00', '2026-03-22 12:00:00'),
('CB946470-8EE1-4A78-B118-D526F282A746', '7D706C26-4E08-4553-886C-14D328984985', '2557FC9B-7599-4EB3-9FA4-7DCD1EAE6EDA', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 5, 8, '2026-03-28 09:53:00', '2026-03-27 12:00:00'),
('8FAABEF1-0299-4DED-BC18-C9FB4CAD8FB2', '7D706C26-4E08-4553-886C-14D328984985', '2557FC9B-7599-4EB3-9FA4-7DCD1EAE6EDA', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 7, 10, '2026-04-01 23:26:00', '2026-04-01 12:00:00'),
('0F6C2509-4757-46AB-BC14-229781CAF927', '7D706C26-4E08-4553-886C-14D328984985', '2557FC9B-7599-4EB3-9FA4-7DCD1EAE6EDA', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 7, 10, '2026-04-07 02:44:00', '2026-04-06 12:00:00'),
('C6FDDF79-57FF-4CC2-89EE-7C1FB405A102', '2BABD30A-0421-4CDD-8FDB-E17FBB085B72', '87C46097-7D04-4AD1-89C2-13967E95301F', N'Skipped', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 4, 4, '2026-03-31 20:45:00', '2026-03-31 12:00:00'),
('40D9F1F5-3617-4282-8CF4-A0402FB1FE4A', '2BABD30A-0421-4CDD-8FDB-E17FBB085B72', '87C46097-7D04-4AD1-89C2-13967E95301F', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 3, 6, '2026-04-04 20:14:00', '2026-04-04 12:00:00'),
('06892EA0-FA12-4002-BF5E-B8D2F41DC447', '2BABD30A-0421-4CDD-8FDB-E17FBB085B72', '87C46097-7D04-4AD1-89C2-13967E95301F', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 3, 5, '2026-04-09 23:10:00', '2026-04-09 12:00:00'),
('D3E75DEA-7A4F-4795-A730-87C50A39DF10', '2BABD30A-0421-4CDD-8FDB-E17FBB085B72', '87C46097-7D04-4AD1-89C2-13967E95301F', N'Skipped', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 4, 4, '2026-04-13 22:28:00', '2026-04-13 12:00:00'),
('D3264E2E-88F5-495A-BF17-893982C8472D', '2BABD30A-0421-4CDD-8FDB-E17FBB085B72', '87C46097-7D04-4AD1-89C2-13967E95301F', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 4, 6, '2026-04-17 20:50:00', '2026-04-17 12:00:00'),
('3D20CB66-71F5-4EC3-BBF3-9A7B9922EB12', '2BABD30A-0421-4CDD-8FDB-E17FBB085B72', '87C46097-7D04-4AD1-89C2-13967E95301F', N'Skipped', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 6, 7, '2026-04-21 21:55:00', '2026-04-21 12:00:00'),
('ED44E170-F988-46A7-8047-73F51CF9C8D6', '2BABD30A-0421-4CDD-8FDB-E17FBB085B72', '87C46097-7D04-4AD1-89C2-13967E95301F', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 5, 7, '2026-04-26 21:01:00', '2026-04-26 12:00:00'),
('4B442D9C-2F56-4296-9FEA-43EACBCCFCFB', '2BABD30A-0421-4CDD-8FDB-E17FBB085B72', '87C46097-7D04-4AD1-89C2-13967E95301F', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 6, 9, '2026-05-01 05:34:00', '2026-04-30 12:00:00'),
('2205F1F0-1513-41FC-9704-D58F020E500F', '7DA8A7AB-0E80-45FC-AE5F-7B3443D8CFB1', '87C46097-7D04-4AD1-89C2-13967E95301F', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 4, 6, '2026-04-01 03:19:00', '2026-03-31 12:00:00'),
('DF6E0F82-D163-4E11-BBEE-DE4BD0595CA5', '7DA8A7AB-0E80-45FC-AE5F-7B3443D8CFB1', '87C46097-7D04-4AD1-89C2-13967E95301F', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 4, 7, '2026-04-20 05:27:00', '2026-04-19 12:00:00'),
('82000022-DC5C-4C87-95EF-0D6E26A84827', '7DA8A7AB-0E80-45FC-AE5F-7B3443D8CFB1', '87C46097-7D04-4AD1-89C2-13967E95301F', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 6, 9, '2026-05-08 23:51:00', '2026-05-08 12:00:00'),
('F63018FD-0AF7-444F-9A66-2C573D1884A5', '7DA8A7AB-0E80-45FC-AE5F-7B3443D8CFB1', '87C46097-7D04-4AD1-89C2-13967E95301F', N'Partial', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 4, 4, '2026-05-27 22:20:00', '2026-05-27 12:00:00'),
('8270D630-D30C-4CFC-83C7-7CC901830976', '7DA8A7AB-0E80-45FC-AE5F-7B3443D8CFB1', '87C46097-7D04-4AD1-89C2-13967E95301F', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 5, 9, '2026-06-16 03:13:00', '2026-06-15 12:00:00'),
('083343C4-EE96-49C3-933E-978511FA6596', 'BC385846-1992-473F-83F8-DC3B2C1F0EC3', '0B875E9D-0EE5-40EF-9CC6-451259EEADB3', N'Skipped', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 3, 3, '2026-01-21 21:00:00', '2026-01-21 12:00:00'),
('887860A8-107E-4372-AD7B-427434FEF7EA', 'BC385846-1992-473F-83F8-DC3B2C1F0EC3', '0B875E9D-0EE5-40EF-9CC6-451259EEADB3', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 3, 6, '2026-01-31 22:21:00', '2026-01-31 12:00:00'),
('586E1A73-9C75-4F03-8CAB-40942B6D3C50', 'BC385846-1992-473F-83F8-DC3B2C1F0EC3', '0B875E9D-0EE5-40EF-9CC6-451259EEADB3', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 5, 8, '2026-02-10 22:39:00', '2026-02-10 12:00:00'),
('280CAEBD-A773-47F1-ADCE-BFE6A77F9559', 'BC385846-1992-473F-83F8-DC3B2C1F0EC3', '0B875E9D-0EE5-40EF-9CC6-451259EEADB3', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 5, 8, '2026-02-20 22:34:00', '2026-02-20 12:00:00'),
('972A5910-118B-42AF-BBEA-7EDCB051BF54', 'BC385846-1992-473F-83F8-DC3B2C1F0EC3', '0B875E9D-0EE5-40EF-9CC6-451259EEADB3', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 5, 7, '2026-03-03 04:01:00', '2026-03-02 12:00:00'),
('34A88A6E-A729-4312-BAC8-EA29D0824D00', '0272206C-5015-4854-901E-CCCED94768D8', '0B875E9D-0EE5-40EF-9CC6-451259EEADB3', N'Skipped', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 4, 4, '2026-01-21 08:50:00', '2026-01-20 12:00:00'),
('366FE44A-7157-49A4-B124-23E5F45E60C0', '0272206C-5015-4854-901E-CCCED94768D8', '0B875E9D-0EE5-40EF-9CC6-451259EEADB3', N'Partial', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 3, 3, '2026-01-30 08:41:00', '2026-01-29 12:00:00'),
('7273DC2F-6D94-40E6-BDC5-A848799AA192', '0272206C-5015-4854-901E-CCCED94768D8', '0B875E9D-0EE5-40EF-9CC6-451259EEADB3', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 5, 7, '2026-02-09 08:21:00', '2026-02-08 12:00:00'),
('E7E2812C-2DB6-4330-8198-6BE935D40D5F', '0272206C-5015-4854-901E-CCCED94768D8', '0B875E9D-0EE5-40EF-9CC6-451259EEADB3', N'Skipped', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 3, 4, '2026-02-17 23:26:00', '2026-02-17 12:00:00'),
('7BD7733F-4CCC-4EA4-B93E-772287B94A34', '0272206C-5015-4854-901E-CCCED94768D8', '0B875E9D-0EE5-40EF-9CC6-451259EEADB3', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 4, 8, '2026-02-27 02:23:00', '2026-02-26 12:00:00'),
('A3977D4E-BBCE-49D3-B063-0708C255EB49', '0272206C-5015-4854-901E-CCCED94768D8', '0B875E9D-0EE5-40EF-9CC6-451259EEADB3', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 5, 7, '2026-03-08 02:39:00', '2026-03-07 12:00:00'),
('51886BFC-4A31-4C42-B64F-2E2C8A233C2B', '0272206C-5015-4854-901E-CCCED94768D8', '0B875E9D-0EE5-40EF-9CC6-451259EEADB3', N'Partial', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 5, 6, '2026-03-18 05:14:00', '2026-03-17 12:00:00'),
('918FB73E-AC4B-4856-99BA-8C2E788997DE', '0272206C-5015-4854-901E-CCCED94768D8', '0B875E9D-0EE5-40EF-9CC6-451259EEADB3', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 7, 10, '2026-03-27 09:27:00', '2026-03-26 12:00:00'),
('2A49E5B5-9C37-4754-88AF-288A6F7B841E', '0272206C-5015-4854-901E-CCCED94768D8', '0B875E9D-0EE5-40EF-9CC6-451259EEADB3', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 6, 8, '2026-04-05 02:48:00', '2026-04-04 12:00:00'),
('A6F546D9-CDB6-445B-AEBC-8CDA9BFF7F94', '2FC27164-204F-4FAE-AF3E-A57E16FBD34A', '0B875E9D-0EE5-40EF-9CC6-451259EEADB3', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 3, 6, '2026-01-24 23:31:00', '2026-01-24 12:00:00'),
('5276876A-9C01-411B-AC89-D7C0F9E78946', '2FC27164-204F-4FAE-AF3E-A57E16FBD34A', '0B875E9D-0EE5-40EF-9CC6-451259EEADB3', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 4, 6, '2026-02-04 21:44:00', '2026-02-04 12:00:00'),
('5801D344-C40C-4382-AA74-2800909F4FF0', '2FC27164-204F-4FAE-AF3E-A57E16FBD34A', '0B875E9D-0EE5-40EF-9CC6-451259EEADB3', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 4, 6, '2026-02-17 06:21:00', '2026-02-16 12:00:00'),
('0C15A836-FCA2-4FF6-B0D8-D6414594D9B7', '2FC27164-204F-4FAE-AF3E-A57E16FBD34A', '0B875E9D-0EE5-40EF-9CC6-451259EEADB3', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 6, 9, '2026-02-28 02:15:00', '2026-02-27 12:00:00'),
('B60E0F2C-DF94-4AF6-AAF1-603614111406', '2FC27164-204F-4FAE-AF3E-A57E16FBD34A', '0B875E9D-0EE5-40EF-9CC6-451259EEADB3', N'Partial', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 5, 6, '2026-03-12 08:40:00', '2026-03-11 12:00:00'),
('215C531B-F82D-43B6-9B68-DD72E7475D44', '2FC27164-204F-4FAE-AF3E-A57E16FBD34A', '0B875E9D-0EE5-40EF-9CC6-451259EEADB3', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 7, 10, '2026-03-22 23:03:00', '2026-03-22 12:00:00'),
('EAAC8328-D2E2-4ACE-81BA-B5D233753EED', '4FF196B4-8097-487A-97BC-D11E035B33DB', '0B875E9D-0EE5-40EF-9CC6-451259EEADB3', N'Partial', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 3, 4, '2026-01-26 20:07:00', '2026-01-26 12:00:00'),
('D116FB8C-2E59-4FEE-A7A0-CCA814E3352C', '4FF196B4-8097-487A-97BC-D11E035B33DB', '0B875E9D-0EE5-40EF-9CC6-451259EEADB3', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 5, 8, '2026-02-09 20:53:00', '2026-02-09 12:00:00'),
('DF7BD765-3225-4963-9643-C07AFE684458', '4FF196B4-8097-487A-97BC-D11E035B33DB', '0B875E9D-0EE5-40EF-9CC6-451259EEADB3', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 3, 7, '2026-02-23 02:40:00', '2026-02-22 12:00:00'),
('A2205C4F-492A-4F8F-A13D-002F5FA55361', '4FF196B4-8097-487A-97BC-D11E035B33DB', '0B875E9D-0EE5-40EF-9CC6-451259EEADB3', N'Partial', N'شعرت بهدوء أكبر بعد التمرين.', 5, 5, '2026-03-09 03:46:00', '2026-03-08 12:00:00'),
('74B2AD03-03C6-4A8E-9404-E011757B2567', '4FF196B4-8097-487A-97BC-D11E035B33DB', '0B875E9D-0EE5-40EF-9CC6-451259EEADB3', N'Partial', N'شعرت بهدوء أكبر بعد التمرين.', 4, 6, '2026-03-22 06:27:00', '2026-03-21 12:00:00'),
('80D03DD1-05D9-40F4-AFEB-F3208703F1F6', '4FF196B4-8097-487A-97BC-D11E035B33DB', '0B875E9D-0EE5-40EF-9CC6-451259EEADB3', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 7, 10, '2026-04-05 01:27:00', '2026-04-04 12:00:00'),
('1E837488-E4F8-48A6-A650-B44257D420EA', '4FF196B4-8097-487A-97BC-D11E035B33DB', '0B875E9D-0EE5-40EF-9CC6-451259EEADB3', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 5, 7, '2026-04-18 02:12:00', '2026-04-17 12:00:00'),
('550C1A2F-18CC-4C9A-9A26-CD4FE68B601D', '1532868E-71BC-489A-B1BA-A958F0F98EC0', 'ED4A6B23-9515-4C85-9110-1C4CC21FAA9E', N'Partial', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 2, 4, '2026-01-17 20:37:00', '2026-01-17 12:00:00'),
('6052C2B0-172F-4F7F-917F-BCBE01FC7154', '1532868E-71BC-489A-B1BA-A958F0F98EC0', 'ED4A6B23-9515-4C85-9110-1C4CC21FAA9E', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 2, 6, '2026-01-24 23:11:00', '2026-01-24 12:00:00'),
('492BA57E-9B2E-4561-85F6-6E1F0BA52890', '1532868E-71BC-489A-B1BA-A958F0F98EC0', 'ED4A6B23-9515-4C85-9110-1C4CC21FAA9E', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 3, 6, '2026-01-31 00:17:00', '2026-01-30 12:00:00'),
('6E00746A-9149-48AF-BCC7-3B8AA57EB242', '1532868E-71BC-489A-B1BA-A958F0F98EC0', 'ED4A6B23-9515-4C85-9110-1C4CC21FAA9E', N'Partial', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 5, 6, '2026-02-07 07:41:00', '2026-02-06 12:00:00'),
('2E12304A-2E57-4EE7-BF1C-D57883D1BEFA', '1532868E-71BC-489A-B1BA-A958F0F98EC0', 'ED4A6B23-9515-4C85-9110-1C4CC21FAA9E', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 6, 10, '2026-02-13 07:22:00', '2026-02-12 12:00:00'),
('1D0D658C-7673-4D2C-889A-BBE45D348C7D', '1532868E-71BC-489A-B1BA-A958F0F98EC0', 'ED4A6B23-9515-4C85-9110-1C4CC21FAA9E', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 6, 10, '2026-02-20 04:24:00', '2026-02-19 12:00:00'),
('C9206A94-BA6B-42E2-A3A2-2888AE5EB2A6', '1532868E-71BC-489A-B1BA-A958F0F98EC0', 'ED4A6B23-9515-4C85-9110-1C4CC21FAA9E', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 7, 9, '2026-02-25 20:28:00', '2026-02-25 12:00:00'),
('BD05E318-1FDB-4774-ADA8-6A361D85CF6C', '1532868E-71BC-489A-B1BA-A958F0F98EC0', 'ED4A6B23-9515-4C85-9110-1C4CC21FAA9E', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 7, 9, '2026-03-05 08:57:00', '2026-03-04 12:00:00'),
('98BF5D41-2A99-4390-92C3-583B9F121E9B', '9A0F4CB3-9DCE-4EA0-8740-3D93108CBB57', 'ED4A6B23-9515-4C85-9110-1C4CC21FAA9E', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 3, 7, '2026-01-17 09:49:00', '2026-01-16 12:00:00'),
('4969F164-FF5B-4AAC-BD51-9BAA7050D41E', '9A0F4CB3-9DCE-4EA0-8740-3D93108CBB57', 'ED4A6B23-9515-4C85-9110-1C4CC21FAA9E', N'Partial', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 3, 3, '2026-02-13 23:50:00', '2026-02-13 12:00:00'),
('BD70B337-6FD0-4E27-A809-CFA8A57562E5', '9A0F4CB3-9DCE-4EA0-8740-3D93108CBB57', 'ED4A6B23-9515-4C85-9110-1C4CC21FAA9E', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 4, 6, '2026-03-14 01:42:00', '2026-03-13 12:00:00'),
('C5F0E1C9-50B6-4412-98A4-67420B9946DA', '9A0F4CB3-9DCE-4EA0-8740-3D93108CBB57', 'ED4A6B23-9515-4C85-9110-1C4CC21FAA9E', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 7, 10, '2026-04-11 09:08:00', '2026-04-10 12:00:00'),
('2FB34D5B-B51A-4054-A89A-6B26D183C8F1', '63043732-ACF6-4C23-BB1D-740E5022F3E3', 'ED4A6B23-9515-4C85-9110-1C4CC21FAA9E', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 2, 6, '2026-01-16 07:51:00', '2026-01-15 12:00:00'),
('6CC29DCA-6909-4A89-85D4-69651980AD1C', '63043732-ACF6-4C23-BB1D-740E5022F3E3', 'ED4A6B23-9515-4C85-9110-1C4CC21FAA9E', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 5, 8, '2026-01-26 23:52:00', '2026-01-26 12:00:00'),
('72CCFEC4-BB1D-48E0-98F9-9B7A8B3AF98D', '63043732-ACF6-4C23-BB1D-740E5022F3E3', 'ED4A6B23-9515-4C85-9110-1C4CC21FAA9E', N'Partial', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 5, 5, '2026-02-06 23:02:00', '2026-02-06 12:00:00'),
('04565099-02EA-4422-A3F0-799207DE9014', '63043732-ACF6-4C23-BB1D-740E5022F3E3', 'ED4A6B23-9515-4C85-9110-1C4CC21FAA9E', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 4, 7, '2026-02-18 04:52:00', '2026-02-17 12:00:00'),
('C053F14D-333F-4046-90E7-DD7A8D284386', '63043732-ACF6-4C23-BB1D-740E5022F3E3', 'ED4A6B23-9515-4C85-9110-1C4CC21FAA9E', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 6, 10, '2026-02-28 00:32:00', '2026-02-27 12:00:00'),
('AF49D4D7-1E23-4ABA-B643-3215EF8B4E0D', '63043732-ACF6-4C23-BB1D-740E5022F3E3', 'ED4A6B23-9515-4C85-9110-1C4CC21FAA9E', N'Partial', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 5, 7, '2026-03-10 23:37:00', '2026-03-10 12:00:00'),
('0988BB8F-999F-47D3-9B3F-E5DD5712CA65', '63043732-ACF6-4C23-BB1D-740E5022F3E3', 'ED4A6B23-9515-4C85-9110-1C4CC21FAA9E', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 5, 9, '2026-03-22 07:53:00', '2026-03-21 12:00:00'),
('C255C4AC-74C9-4186-BC5D-2A634CCDBFB9', 'F55D45BA-36D7-4A39-9EC4-9B86C3C3A7C8', 'ED4A6B23-9515-4C85-9110-1C4CC21FAA9E', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 4, 6, '2026-01-17 06:59:00', '2026-01-16 12:00:00'),
('8F95181C-56A1-4139-98C7-790FFFB0869A', 'F55D45BA-36D7-4A39-9EC4-9B86C3C3A7C8', 'ED4A6B23-9515-4C85-9110-1C4CC21FAA9E', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 5, 7, '2026-01-24 21:36:00', '2026-01-24 12:00:00'),
('4BC31258-40E3-4BB7-93CF-4DDC5B9E7345', 'F55D45BA-36D7-4A39-9EC4-9B86C3C3A7C8', 'ED4A6B23-9515-4C85-9110-1C4CC21FAA9E', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 3, 7, '2026-02-01 03:52:00', '2026-01-31 12:00:00'),
('5DFCFE2D-064A-4AF7-85C5-42ECE0E2B1B8', 'F55D45BA-36D7-4A39-9EC4-9B86C3C3A7C8', 'ED4A6B23-9515-4C85-9110-1C4CC21FAA9E', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 6, 8, '2026-02-09 03:57:00', '2026-02-08 12:00:00'),
('E3AD154A-C2CF-41D0-9072-9409AD5B631B', 'F55D45BA-36D7-4A39-9EC4-9B86C3C3A7C8', 'ED4A6B23-9515-4C85-9110-1C4CC21FAA9E', N'Partial', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 5, 6, '2026-02-15 22:55:00', '2026-02-15 12:00:00'),
('FD91CC8C-76C7-4CFD-8544-EEC8341353E5', 'F55D45BA-36D7-4A39-9EC4-9B86C3C3A7C8', 'ED4A6B23-9515-4C85-9110-1C4CC21FAA9E', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 5, 8, '2026-02-24 05:24:00', '2026-02-23 12:00:00'),
('693DB9F3-C113-4CF3-96CC-9129FAFEF4A7', 'E4CF0A16-C6DE-4B55-8AFA-46A17160D3F7', '69F104EE-9B40-40CF-A029-EC2A532693FA', N'Partial', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 3, 5, '2026-04-15 20:36:00', '2026-04-15 12:00:00'),
('712EE742-B0F8-4E27-BE12-0EBD886B93AF', 'E4CF0A16-C6DE-4B55-8AFA-46A17160D3F7', '69F104EE-9B40-40CF-A029-EC2A532693FA', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 3, 6, '2026-04-29 06:51:00', '2026-04-28 12:00:00'),
('EBB57B23-927A-4A4A-9AF1-6732DAF7D301', 'E4CF0A16-C6DE-4B55-8AFA-46A17160D3F7', '69F104EE-9B40-40CF-A029-EC2A532693FA', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 5, 9, '2026-05-11 08:39:00', '2026-05-10 12:00:00'),
('DEA07415-4B48-40DB-8D53-918150C7A500', 'E4CF0A16-C6DE-4B55-8AFA-46A17160D3F7', '69F104EE-9B40-40CF-A029-EC2A532693FA', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 6, 10, '2026-05-24 06:59:00', '2026-05-23 12:00:00'),
('30103148-6464-40C7-9471-EF8963C4C860', 'E4CF0A16-C6DE-4B55-8AFA-46A17160D3F7', '69F104EE-9B40-40CF-A029-EC2A532693FA', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 5, 8, '2026-06-04 22:32:00', '2026-06-04 12:00:00'),
('78D9A3DC-CDC7-461D-ADD6-14E7CED969AC', 'E4CF0A16-C6DE-4B55-8AFA-46A17160D3F7', '69F104EE-9B40-40CF-A029-EC2A532693FA', N'Skipped', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 6, 7, '2026-06-18 09:48:00', '2026-06-17 12:00:00'),
('4FA4C7CE-7759-44C5-9302-530D060E2A02', 'D1686C83-CDED-4630-9980-0FAC3BED7BE3', '69F104EE-9B40-40CF-A029-EC2A532693FA', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 2, 6, '2026-04-13 08:39:00', '2026-04-12 12:00:00'),
('EFAFC11C-1503-4EF7-9B96-BA7C414E2021', 'D1686C83-CDED-4630-9980-0FAC3BED7BE3', '69F104EE-9B40-40CF-A029-EC2A532693FA', N'Skipped', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 4, 3, '2026-04-27 05:21:00', '2026-04-26 12:00:00'),
('129D810B-6AE3-45B5-9E1A-9539B7893880', 'D1686C83-CDED-4630-9980-0FAC3BED7BE3', '69F104EE-9B40-40CF-A029-EC2A532693FA', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 6, 8, '2026-05-10 23:46:00', '2026-05-10 12:00:00'),
('CD261061-639F-49C8-A11F-B6086BADD9C8', 'D1686C83-CDED-4630-9980-0FAC3BED7BE3', '69F104EE-9B40-40CF-A029-EC2A532693FA', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 5, 9, '2026-05-24 21:09:00', '2026-05-24 12:00:00'),
('C73CAEA7-D7C4-4564-B0A0-0EC07083AC2E', 'D1686C83-CDED-4630-9980-0FAC3BED7BE3', '69F104EE-9B40-40CF-A029-EC2A532693FA', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 5, 9, '2026-06-08 00:00:00', '2026-06-07 12:00:00'),
('3B52F303-86DC-41D8-AB6B-51D090B331AF', '10B8BCC6-629F-4A0C-B704-1F70570E3349', '69F104EE-9B40-40CF-A029-EC2A532693FA', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 3, 7, '2026-04-12 04:25:00', '2026-04-11 12:00:00'),
('9B851028-E4BA-4EB1-AD21-20961E8F2EC0', '10B8BCC6-629F-4A0C-B704-1F70570E3349', '69F104EE-9B40-40CF-A029-EC2A532693FA', N'Partial', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 5, 5, '2026-04-19 06:34:00', '2026-04-18 12:00:00'),
('7FF96398-652B-427D-A38E-F0DBB2D4348A', '10B8BCC6-629F-4A0C-B704-1F70570E3349', '69F104EE-9B40-40CF-A029-EC2A532693FA', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 3, 5, '2026-04-24 22:19:00', '2026-04-24 12:00:00'),
('2C4E7461-09EF-4CBB-8D9A-D58101AAA375', '10B8BCC6-629F-4A0C-B704-1F70570E3349', '69F104EE-9B40-40CF-A029-EC2A532693FA', N'Partial', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 4, 4, '2026-05-02 00:38:00', '2026-05-01 12:00:00'),
('D9E33649-98CE-4EFE-B227-A3E460E3E77A', '10B8BCC6-629F-4A0C-B704-1F70570E3349', '69F104EE-9B40-40CF-A029-EC2A532693FA', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 4, 7, '2026-05-08 04:24:00', '2026-05-07 12:00:00'),
('520D4BF9-522A-43C7-831F-7CD08DF6726A', '10B8BCC6-629F-4A0C-B704-1F70570E3349', '69F104EE-9B40-40CF-A029-EC2A532693FA', N'Partial', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 7, 8, '2026-05-15 01:27:00', '2026-05-14 12:00:00'),
('7E989BDF-AF8E-4576-A4CA-B99E28335E3A', '7D9357C8-7140-45F4-961C-33FE9FA9D76F', '56A8CAE5-A2FA-4149-B2D0-3E5B35F37D3B', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 3, 6, '2026-05-06 07:04:00', '2026-05-05 12:00:00'),
('315BC48C-C4AA-4240-9AB8-A9A5F2C79231', '7D9357C8-7140-45F4-961C-33FE9FA9D76F', '56A8CAE5-A2FA-4149-B2D0-3E5B35F37D3B', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 3, 7, '2026-05-14 21:18:00', '2026-05-14 12:00:00'),
('61FB3A34-38CF-4F7F-95AB-BB3D37C529CC', '7D9357C8-7140-45F4-961C-33FE9FA9D76F', '56A8CAE5-A2FA-4149-B2D0-3E5B35F37D3B', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 4, 6, '2026-05-25 01:05:00', '2026-05-24 12:00:00'),
('CFC6F886-483F-4A04-8AE1-DE8B33461431', '7D9357C8-7140-45F4-961C-33FE9FA9D76F', '56A8CAE5-A2FA-4149-B2D0-3E5B35F37D3B', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 6, 10, '2026-06-03 02:31:00', '2026-06-02 12:00:00'),
('2F5E861D-C7C5-4495-A289-FE24B415EA33', '7D9357C8-7140-45F4-961C-33FE9FA9D76F', '56A8CAE5-A2FA-4149-B2D0-3E5B35F37D3B', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 7, 9, '2026-06-11 21:56:00', '2026-06-11 12:00:00'),
('BCE76BA1-A963-43DD-8CDA-0CC5CAA2D393', '930AB235-378D-483A-9FE8-F603BF555724', '56A8CAE5-A2FA-4149-B2D0-3E5B35F37D3B', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 3, 6, '2026-05-03 05:58:00', '2026-05-02 12:00:00'),
('5501A84F-BD9C-4387-B1F1-41A2CE13EDAE', '930AB235-378D-483A-9FE8-F603BF555724', '56A8CAE5-A2FA-4149-B2D0-3E5B35F37D3B', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 5, 9, '2026-05-22 00:41:00', '2026-05-21 12:00:00'),
('21D70340-1100-42C3-85C2-C9ADB6BC6B05', '930AB235-378D-483A-9FE8-F603BF555724', '56A8CAE5-A2FA-4149-B2D0-3E5B35F37D3B', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 5, 9, '2026-06-10 03:50:00', '2026-06-09 12:00:00'),
('233480A0-395C-4A4C-BAA8-A6E5F2C40C71', '930AB235-378D-483A-9FE8-F603BF555724', '56A8CAE5-A2FA-4149-B2D0-3E5B35F37D3B', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 6, 10, '2026-06-29 09:34:00', '2026-06-28 12:00:00'),
('4A4B6154-BF01-4514-9B42-A479D1E775D2', '4CFE6AD8-547A-42E4-BBDF-1EA89AE328E4', '56A8CAE5-A2FA-4149-B2D0-3E5B35F37D3B', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 4, 7, '2026-05-05 03:39:00', '2026-05-04 12:00:00'),
('5D615BA7-9364-499E-8130-734A5AB23F31', '4CFE6AD8-547A-42E4-BBDF-1EA89AE328E4', '56A8CAE5-A2FA-4149-B2D0-3E5B35F37D3B', N'Skipped', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 4, 5, '2026-05-16 20:17:00', '2026-05-16 12:00:00'),
('9E929E3B-5266-4ED6-A76E-78B8F7C0E20D', '4CFE6AD8-547A-42E4-BBDF-1EA89AE328E4', '56A8CAE5-A2FA-4149-B2D0-3E5B35F37D3B', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 3, 7, '2026-05-29 04:04:00', '2026-05-28 12:00:00'),
('A58B4C45-46EE-4716-9546-2FC16CD9EC74', '4CFE6AD8-547A-42E4-BBDF-1EA89AE328E4', '56A8CAE5-A2FA-4149-B2D0-3E5B35F37D3B', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 4, 8, '2026-06-09 01:51:00', '2026-06-08 12:00:00'),
('1BE35BCD-5530-4CBD-BA4D-C299FDA015CE', '4CFE6AD8-547A-42E4-BBDF-1EA89AE328E4', '56A8CAE5-A2FA-4149-B2D0-3E5B35F37D3B', N'Skipped', N'شعرت بهدوء أكبر بعد التمرين.', 6, 6, '2026-06-20 21:57:00', '2026-06-20 12:00:00'),
('13521009-F8E8-4E74-AFEE-D1C90394BC23', '4CFE6AD8-547A-42E4-BBDF-1EA89AE328E4', '56A8CAE5-A2FA-4149-B2D0-3E5B35F37D3B', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 6, 9, '2026-07-03 06:05:00', '2026-07-02 12:00:00'),
('FF7067F0-1A38-45F1-9030-309BCA471005', '2448A917-391B-4F95-BF78-E5F0186D8B66', '56A8CAE5-A2FA-4149-B2D0-3E5B35F37D3B', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 4, 6, '2026-05-03 21:18:00', '2026-05-03 12:00:00'),
('6487460A-0B5D-4E5F-BA06-7072AE169892', '2448A917-391B-4F95-BF78-E5F0186D8B66', '56A8CAE5-A2FA-4149-B2D0-3E5B35F37D3B', N'Partial', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 3, 4, '2026-05-11 05:47:00', '2026-05-10 12:00:00'),
('A0EB7BF2-35EA-428F-A173-7EE48F68110B', '2448A917-391B-4F95-BF78-E5F0186D8B66', '56A8CAE5-A2FA-4149-B2D0-3E5B35F37D3B', N'Partial', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 4, 4, '2026-05-18 05:17:00', '2026-05-17 12:00:00'),
('DC95C14A-BF0D-4B6B-8EC4-4300D12BCEB3', '2448A917-391B-4F95-BF78-E5F0186D8B66', '56A8CAE5-A2FA-4149-B2D0-3E5B35F37D3B', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 4, 8, '2026-05-24 02:02:00', '2026-05-23 12:00:00'),
('183FDBBB-6674-482D-A579-9C6A581DE0FC', '2448A917-391B-4F95-BF78-E5F0186D8B66', '56A8CAE5-A2FA-4149-B2D0-3E5B35F37D3B', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 5, 9, '2026-05-30 23:41:00', '2026-05-30 12:00:00'),
('1B06974B-12AA-4410-B3A3-73C4533A2051', '2448A917-391B-4F95-BF78-E5F0186D8B66', '56A8CAE5-A2FA-4149-B2D0-3E5B35F37D3B', N'Skipped', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 4, 4, '2026-06-07 09:55:00', '2026-06-06 12:00:00'),
('4C2733AA-E3D0-4333-B93F-5761C9DB7C1E', '2448A917-391B-4F95-BF78-E5F0186D8B66', '56A8CAE5-A2FA-4149-B2D0-3E5B35F37D3B', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 5, 8, '2026-06-14 01:01:00', '2026-06-13 12:00:00'),
('E7CAF94B-053C-4011-ADE2-B5CBBA317216', '2448A917-391B-4F95-BF78-E5F0186D8B66', '56A8CAE5-A2FA-4149-B2D0-3E5B35F37D3B', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 7, 9, '2026-06-20 05:10:00', '2026-06-19 12:00:00'),
('B3076C3B-F369-45C1-B84B-E4B56EF29188', '2448A917-391B-4F95-BF78-E5F0186D8B66', '56A8CAE5-A2FA-4149-B2D0-3E5B35F37D3B', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 5, 8, '2026-06-26 22:46:00', '2026-06-26 12:00:00'),
('AE721557-E0EB-4081-9ADD-0BB85B896319', '130124E3-2CC6-4E58-BDE1-EE6A36C5EFCA', '85B46DDD-B1A6-4148-BB18-D7DB4172EAF2', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 4, 6, '2025-09-15 06:34:00', '2025-09-14 12:00:00'),
('A7D3086B-4A13-4DDE-8DC6-31169E1BCA57', '130124E3-2CC6-4E58-BDE1-EE6A36C5EFCA', '85B46DDD-B1A6-4148-BB18-D7DB4172EAF2', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 3, 5, '2025-09-22 06:53:00', '2025-09-21 12:00:00'),
('E81AFAA1-E6DA-4A91-B907-3194CE551619', '130124E3-2CC6-4E58-BDE1-EE6A36C5EFCA', '85B46DDD-B1A6-4148-BB18-D7DB4172EAF2', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 3, 5, '2025-09-28 06:36:00', '2025-09-27 12:00:00'),
('18D98F1A-465C-4334-A37C-30770BCE725C', '130124E3-2CC6-4E58-BDE1-EE6A36C5EFCA', '85B46DDD-B1A6-4148-BB18-D7DB4172EAF2', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 5, 7, '2025-10-05 00:15:00', '2025-10-04 12:00:00'),
('D0729758-00E7-47B8-9B6F-0228BB77FC41', '130124E3-2CC6-4E58-BDE1-EE6A36C5EFCA', '85B46DDD-B1A6-4148-BB18-D7DB4172EAF2', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 6, 8, '2025-10-10 23:40:00', '2025-10-10 12:00:00'),
('F4EAF5C6-4578-4CDC-8A19-9685093C7E1F', '130124E3-2CC6-4E58-BDE1-EE6A36C5EFCA', '85B46DDD-B1A6-4148-BB18-D7DB4172EAF2', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 6, 8, '2025-10-17 20:49:00', '2025-10-17 12:00:00'),
('8AC53FD9-312D-47F8-A231-112B6FECE105', '130124E3-2CC6-4E58-BDE1-EE6A36C5EFCA', '85B46DDD-B1A6-4148-BB18-D7DB4172EAF2', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 6, 8, '2025-10-24 06:12:00', '2025-10-23 12:00:00'),
('22A7B122-DBF5-45B4-93E6-1A7673FD602E', '7E63C494-9958-4245-A639-C034022DFB13', '85B46DDD-B1A6-4148-BB18-D7DB4172EAF2', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 3, 6, '2025-09-17 02:54:00', '2025-09-16 12:00:00'),
('08E93F80-56DC-45D8-A457-C132234453BA', '7E63C494-9958-4245-A639-C034022DFB13', '85B46DDD-B1A6-4148-BB18-D7DB4172EAF2', N'Partial', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 5, 6, '2025-09-22 22:26:00', '2025-09-22 12:00:00'),
('230D612F-E323-40B8-9759-EB0782FF7CB1', '7E63C494-9958-4245-A639-C034022DFB13', '85B46DDD-B1A6-4148-BB18-D7DB4172EAF2', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 3, 5, '2025-09-28 02:47:00', '2025-09-27 12:00:00'),
('E4618B69-9036-4F3B-8A56-E2FC75B5AA75', '7E63C494-9958-4245-A639-C034022DFB13', '85B46DDD-B1A6-4148-BB18-D7DB4172EAF2', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 5, 8, '2025-10-04 01:59:00', '2025-10-03 12:00:00'),
('ECEB1EAD-1184-4D45-AC34-8F45EE37D018', '7E63C494-9958-4245-A639-C034022DFB13', '85B46DDD-B1A6-4148-BB18-D7DB4172EAF2', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 5, 7, '2025-10-10 03:56:00', '2025-10-09 12:00:00'),
('84DE6965-B6A4-47DE-8792-2F9174E2455D', '7E63C494-9958-4245-A639-C034022DFB13', '85B46DDD-B1A6-4148-BB18-D7DB4172EAF2', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 6, 10, '2025-10-15 04:40:00', '2025-10-14 12:00:00'),
('80B8DDEF-4D5F-458F-85E5-9776399F8142', '7E63C494-9958-4245-A639-C034022DFB13', '85B46DDD-B1A6-4148-BB18-D7DB4172EAF2', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 5, 9, '2025-10-20 23:59:00', '2025-10-20 12:00:00'),
('68742BD0-3C5A-4D05-9E92-5DA901FF77E9', '58A102E0-817C-4EA4-88FB-4ACA0D89AC21', 'D5AF7A7D-26C9-4CD6-854B-11C4F6A65218', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 3, 5, '2025-10-09 02:36:00', '2025-10-08 12:00:00'),
('0F16034F-5984-451B-A442-A6C811053188', '58A102E0-817C-4EA4-88FB-4ACA0D89AC21', 'D5AF7A7D-26C9-4CD6-854B-11C4F6A65218', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 4, 8, '2025-10-12 20:34:00', '2025-10-12 12:00:00'),
('9A3634BF-4103-43F4-AC0F-F0A62CD33B95', '58A102E0-817C-4EA4-88FB-4ACA0D89AC21', 'D5AF7A7D-26C9-4CD6-854B-11C4F6A65218', N'Partial', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 4, 6, '2025-10-18 08:35:00', '2025-10-17 12:00:00'),
('49B6A55F-D1C4-4332-86F4-DED101D2F2DC', '58A102E0-817C-4EA4-88FB-4ACA0D89AC21', 'D5AF7A7D-26C9-4CD6-854B-11C4F6A65218', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 5, 9, '2025-10-21 22:18:00', '2025-10-21 12:00:00'),
('84214EAC-0D85-4AC1-BF6F-D7088DC74F3C', '58A102E0-817C-4EA4-88FB-4ACA0D89AC21', 'D5AF7A7D-26C9-4CD6-854B-11C4F6A65218', N'Partial', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 5, 7, '2025-10-26 02:36:00', '2025-10-25 12:00:00'),
('47FAFE1C-DEEF-46DD-95AE-F7845C617478', '58A102E0-817C-4EA4-88FB-4ACA0D89AC21', 'D5AF7A7D-26C9-4CD6-854B-11C4F6A65218', N'Partial', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 4, 5, '2025-10-30 05:21:00', '2025-10-29 12:00:00'),
('CC3BDDDE-CB37-4492-829D-91DC3D86F5FB', '58A102E0-817C-4EA4-88FB-4ACA0D89AC21', 'D5AF7A7D-26C9-4CD6-854B-11C4F6A65218', N'Partial', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 6, 7, '2025-11-04 03:59:00', '2025-11-03 12:00:00'),
('A49FE41C-B9EF-410B-A675-AB8F3C7B60CB', '58A102E0-817C-4EA4-88FB-4ACA0D89AC21', 'D5AF7A7D-26C9-4CD6-854B-11C4F6A65218', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 6, 8, '2025-11-08 00:41:00', '2025-11-07 12:00:00'),
('BBC86A25-E23C-4B5C-84B3-3E1721C1AE31', '58A102E0-817C-4EA4-88FB-4ACA0D89AC21', 'D5AF7A7D-26C9-4CD6-854B-11C4F6A65218', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 6, 9, '2025-11-12 05:39:00', '2025-11-11 12:00:00'),
('83C21DAD-9126-4A17-99F6-1E24F173FCDC', '01345114-20DE-4806-A6D4-2E9FB2011EF3', 'D5AF7A7D-26C9-4CD6-854B-11C4F6A65218', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 3, 5, '2025-10-09 00:37:00', '2025-10-08 12:00:00'),
('E823C09B-74D4-49E9-BCA4-20B0EBAB781E', '01345114-20DE-4806-A6D4-2E9FB2011EF3', 'D5AF7A7D-26C9-4CD6-854B-11C4F6A65218', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 3, 5, '2025-10-17 03:05:00', '2025-10-16 12:00:00'),
('4EBF51C5-58BB-4D7E-9DB6-ABF3ED02A2F8', '01345114-20DE-4806-A6D4-2E9FB2011EF3', 'D5AF7A7D-26C9-4CD6-854B-11C4F6A65218', N'Skipped', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 4, 4, '2025-10-23 22:53:00', '2025-10-23 12:00:00'),
('53F13950-47AC-4C93-A6F6-3BEA1675110D', '01345114-20DE-4806-A6D4-2E9FB2011EF3', 'D5AF7A7D-26C9-4CD6-854B-11C4F6A65218', N'Partial', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 4, 4, '2025-11-01 05:36:00', '2025-10-31 12:00:00'),
('7499A469-AD16-461F-A463-1A1C3EC6D7DA', '01345114-20DE-4806-A6D4-2E9FB2011EF3', 'D5AF7A7D-26C9-4CD6-854B-11C4F6A65218', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 5, 9, '2025-11-07 23:21:00', '2025-11-07 12:00:00'),
('E51F9DC5-4442-4829-9860-3831AD434AE7', '638930A8-25A5-4C0E-A532-04D209CF79D2', 'D5AF7A7D-26C9-4CD6-854B-11C4F6A65218', N'Partial', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 3, 5, '2025-10-06 00:23:00', '2025-10-05 12:00:00'),
('62238D5B-0359-4707-BCA6-292665DFBC4B', '638930A8-25A5-4C0E-A532-04D209CF79D2', 'D5AF7A7D-26C9-4CD6-854B-11C4F6A65218', N'Skipped', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 3, 3, '2025-10-11 20:50:00', '2025-10-11 12:00:00'),
('C19324D5-8EE7-4763-A9DD-266E8B6BCD3D', '638930A8-25A5-4C0E-A532-04D209CF79D2', 'D5AF7A7D-26C9-4CD6-854B-11C4F6A65218', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 4, 8, '2025-10-17 08:38:00', '2025-10-16 12:00:00'),
('65B807E7-3CD5-4C49-BF86-1B2CA9800164', '638930A8-25A5-4C0E-A532-04D209CF79D2', 'D5AF7A7D-26C9-4CD6-854B-11C4F6A65218', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 4, 7, '2025-10-23 05:50:00', '2025-10-22 12:00:00'),
('6595C36D-85DF-44D0-85FD-6B8E3D6015AE', '638930A8-25A5-4C0E-A532-04D209CF79D2', 'D5AF7A7D-26C9-4CD6-854B-11C4F6A65218', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 5, 9, '2025-10-28 04:09:00', '2025-10-27 12:00:00'),
('49CF05A7-B5A2-4DAC-82FB-26B0AD1E8405', '638930A8-25A5-4C0E-A532-04D209CF79D2', 'D5AF7A7D-26C9-4CD6-854B-11C4F6A65218', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 5, 8, '2025-11-02 23:45:00', '2025-11-02 12:00:00'),
('FA08EE83-432D-40FE-B13F-092A8DD6AEAF', '638930A8-25A5-4C0E-A532-04D209CF79D2', 'D5AF7A7D-26C9-4CD6-854B-11C4F6A65218', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 7, 10, '2025-11-08 02:45:00', '2025-11-07 12:00:00'),
('A55B1CAD-C854-4DF3-A6F1-AEAE0E429A65', 'FB6CC26C-CD98-4F17-A02F-6A47A417D98F', 'D5AF7A7D-26C9-4CD6-854B-11C4F6A65218', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 4, 7, '2025-10-06 04:40:00', '2025-10-05 12:00:00'),
('5D769016-6264-4E52-8783-E9F6A51DBDE8', 'FB6CC26C-CD98-4F17-A02F-6A47A417D98F', 'D5AF7A7D-26C9-4CD6-854B-11C4F6A65218', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 3, 6, '2025-10-13 21:52:00', '2025-10-13 12:00:00'),
('F03C3ED8-D6C1-4255-AE80-6A7DFE736A29', 'FB6CC26C-CD98-4F17-A02F-6A47A417D98F', 'D5AF7A7D-26C9-4CD6-854B-11C4F6A65218', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 4, 6, '2025-10-22 04:35:00', '2025-10-21 12:00:00'),
('DE7B689D-FD6D-4213-BF68-ED3D552B3FDB', 'FB6CC26C-CD98-4F17-A02F-6A47A417D98F', 'D5AF7A7D-26C9-4CD6-854B-11C4F6A65218', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 3, 5, '2025-10-30 01:23:00', '2025-10-29 12:00:00'),
('78A931A1-14C2-4E13-ACE8-B21C7861D85F', 'FB6CC26C-CD98-4F17-A02F-6A47A417D98F', 'D5AF7A7D-26C9-4CD6-854B-11C4F6A65218', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 4, 6, '2025-11-06 22:05:00', '2025-11-06 12:00:00'),
('FC0436B8-CA3C-443D-B39A-F87A9883F715', 'FB6CC26C-CD98-4F17-A02F-6A47A417D98F', 'D5AF7A7D-26C9-4CD6-854B-11C4F6A65218', N'Partial', N'شعرت بهدوء أكبر بعد التمرين.', 6, 6, '2025-11-15 03:05:00', '2025-11-14 12:00:00'),
('F3E0C8BB-8D06-425D-A8A0-468F5EDE8FC5', 'FB6CC26C-CD98-4F17-A02F-6A47A417D98F', 'D5AF7A7D-26C9-4CD6-854B-11C4F6A65218', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 6, 10, '2025-11-22 20:09:00', '2025-11-22 12:00:00'),
('CAE6A94E-A8DA-443C-BC1B-81E655D96EE9', 'FB6CC26C-CD98-4F17-A02F-6A47A417D98F', 'D5AF7A7D-26C9-4CD6-854B-11C4F6A65218', N'Partial', N'شعرت بهدوء أكبر بعد التمرين.', 6, 6, '2025-11-30 23:27:00', '2025-11-30 12:00:00'),
('AE6A7B17-C70B-41D7-AAB3-C10FB0EBA5A2', 'FB6CC26C-CD98-4F17-A02F-6A47A417D98F', 'D5AF7A7D-26C9-4CD6-854B-11C4F6A65218', N'Partial', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 5, 5, '2025-12-09 09:06:00', '2025-12-08 12:00:00'),
('CAED8816-651D-4DCA-B1AF-8B19D648A05F', '1A70C056-4875-4085-B56C-4F1F95E1EAA3', 'AEB38627-5539-4776-8AEC-69F91DD159F1', N'Partial', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 2, 2, '2025-11-19 00:34:00', '2025-11-18 12:00:00'),
('61BF2097-AD75-4D30-B653-68467B7EAA83', '1A70C056-4875-4085-B56C-4F1F95E1EAA3', 'AEB38627-5539-4776-8AEC-69F91DD159F1', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 3, 6, '2025-11-25 08:46:00', '2025-11-24 12:00:00'),
('A780EE30-26CE-4106-8E06-10308C0E73F3', '1A70C056-4875-4085-B56C-4F1F95E1EAA3', 'AEB38627-5539-4776-8AEC-69F91DD159F1', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 5, 7, '2025-11-30 07:45:00', '2025-11-29 12:00:00'),
('9CB12EC6-C138-453A-B0AC-09C02E9919C4', '1A70C056-4875-4085-B56C-4F1F95E1EAA3', 'AEB38627-5539-4776-8AEC-69F91DD159F1', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 5, 9, '2025-12-06 01:42:00', '2025-12-05 12:00:00'),
('D34EC7B3-CEE7-4715-9403-443DCBE5CBFC', '1A70C056-4875-4085-B56C-4F1F95E1EAA3', 'AEB38627-5539-4776-8AEC-69F91DD159F1', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 5, 7, '2025-12-11 22:56:00', '2025-12-11 12:00:00'),
('AB6CFFC3-C453-4A55-81AC-E263113FD408', '1A70C056-4875-4085-B56C-4F1F95E1EAA3', 'AEB38627-5539-4776-8AEC-69F91DD159F1', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 4, 7, '2025-12-17 21:10:00', '2025-12-17 12:00:00'),
('51ABC03E-2FA6-4B8F-9F55-3D73946B5074', '1A70C056-4875-4085-B56C-4F1F95E1EAA3', 'AEB38627-5539-4776-8AEC-69F91DD159F1', N'Skipped', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 6, 7, '2025-12-23 06:35:00', '2025-12-22 12:00:00'),
('4CFE95E2-A647-43B3-9F2C-3DB9027F24F6', '1A70C056-4875-4085-B56C-4F1F95E1EAA3', 'AEB38627-5539-4776-8AEC-69F91DD159F1', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 7, 10, '2025-12-28 22:46:00', '2025-12-28 12:00:00'),
('CCDF8B9F-DE4C-42F4-B151-67A073452267', 'A238C47A-F5DC-4ECF-905B-77F2972113F5', 'AEB38627-5539-4776-8AEC-69F91DD159F1', N'Partial', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 3, 3, '2025-11-22 07:55:00', '2025-11-21 12:00:00'),
('9D83A3C2-D3D8-4667-92BF-84388D2C2107', 'A238C47A-F5DC-4ECF-905B-77F2972113F5', 'AEB38627-5539-4776-8AEC-69F91DD159F1', N'Skipped', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 4, 3, '2025-12-06 09:15:00', '2025-12-05 12:00:00'),
('CD05B339-E199-4889-9539-8218C772D1EE', 'A238C47A-F5DC-4ECF-905B-77F2972113F5', 'AEB38627-5539-4776-8AEC-69F91DD159F1', N'Partial', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 6, 8, '2025-12-19 04:12:00', '2025-12-18 12:00:00'),
('704FC9C6-80F9-4DDC-A15F-88A9DE902755', 'A238C47A-F5DC-4ECF-905B-77F2972113F5', 'AEB38627-5539-4776-8AEC-69F91DD159F1', N'Partial', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 6, 6, '2026-01-02 03:46:00', '2026-01-01 12:00:00'),
('733F3EC0-09E1-49DE-B175-8AB891AC9AE1', '78A3DC62-30C3-4D07-8420-686EB7F8E71B', 'AEB38627-5539-4776-8AEC-69F91DD159F1', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 3, 6, '2025-11-17 21:19:00', '2025-11-17 12:00:00'),
('43B3AB85-0A74-4654-8CB6-8891F325394B', '78A3DC62-30C3-4D07-8420-686EB7F8E71B', 'AEB38627-5539-4776-8AEC-69F91DD159F1', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 3, 5, '2025-12-01 22:04:00', '2025-12-01 12:00:00'),
('5FF17C66-3491-4A29-8CF1-4837892F7336', '78A3DC62-30C3-4D07-8420-686EB7F8E71B', 'AEB38627-5539-4776-8AEC-69F91DD159F1', N'Partial', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 3, 3, '2025-12-15 03:23:00', '2025-12-14 12:00:00'),
('972E2A5D-4792-47DB-81D9-DE148F482655', '78A3DC62-30C3-4D07-8420-686EB7F8E71B', 'AEB38627-5539-4776-8AEC-69F91DD159F1', N'Partial', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 6, 6, '2025-12-28 23:37:00', '2025-12-28 12:00:00'),
('D0CCA53C-5447-4D23-BFE5-9CC87617A20B', '78A3DC62-30C3-4D07-8420-686EB7F8E71B', 'AEB38627-5539-4776-8AEC-69F91DD159F1', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 6, 8, '2026-01-11 10:00:00', '2026-01-10 12:00:00'),
('159E4256-66A3-4645-95E6-52E8A3A5CC1B', '78A3DC62-30C3-4D07-8420-686EB7F8E71B', 'AEB38627-5539-4776-8AEC-69F91DD159F1', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 6, 9, '2026-01-25 04:16:00', '2026-01-24 12:00:00'),
('F614E074-75BC-4F88-9E57-B79F760B5778', '78A3DC62-30C3-4D07-8420-686EB7F8E71B', 'AEB38627-5539-4776-8AEC-69F91DD159F1', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 7, 10, '2026-02-07 01:56:00', '2026-02-06 12:00:00'),
('59C2702C-B015-42F8-8C75-D4981500C799', '6CF486F3-E92B-4AD0-861F-61099A4ADF90', 'AEB38627-5539-4776-8AEC-69F91DD159F1', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 2, 6, '2025-11-21 01:14:00', '2025-11-20 12:00:00'),
('9C11AC70-4F44-4936-BCC3-9E2D089E3BD8', '6CF486F3-E92B-4AD0-861F-61099A4ADF90', 'AEB38627-5539-4776-8AEC-69F91DD159F1', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 4, 7, '2025-12-02 07:13:00', '2025-12-01 12:00:00'),
('511641AB-C884-492C-9751-804B2A8E049B', '6CF486F3-E92B-4AD0-861F-61099A4ADF90', 'AEB38627-5539-4776-8AEC-69F91DD159F1', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 3, 6, '2025-12-12 08:55:00', '2025-12-11 12:00:00'),
('CA88002D-96BB-4C85-A0D2-D4D757835420', '6CF486F3-E92B-4AD0-861F-61099A4ADF90', 'AEB38627-5539-4776-8AEC-69F91DD159F1', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 4, 8, '2025-12-22 21:38:00', '2025-12-22 12:00:00'),
('5262EA03-55B9-408C-901B-C2EE4DCFECE8', '6CF486F3-E92B-4AD0-861F-61099A4ADF90', 'AEB38627-5539-4776-8AEC-69F91DD159F1', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 6, 10, '2026-01-02 05:21:00', '2026-01-01 12:00:00'),
('9B604C90-1429-46E0-8D46-A6D48F66D1F2', '6CF486F3-E92B-4AD0-861F-61099A4ADF90', 'AEB38627-5539-4776-8AEC-69F91DD159F1', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 6, 9, '2026-01-13 07:01:00', '2026-01-12 12:00:00'),
('474C57B9-B560-487E-B135-E190C81AAE2B', '6CF486F3-E92B-4AD0-861F-61099A4ADF90', 'AEB38627-5539-4776-8AEC-69F91DD159F1', N'Skipped', N'شعرت بهدوء أكبر بعد التمرين.', 6, 7, '2026-01-23 07:13:00', '2026-01-22 12:00:00'),
('7BE835B9-BAF2-4022-BB66-5FFAC587E2F9', '293E35A0-25B0-43B6-9574-709F14FE8564', '00CC925F-9671-4970-ADC6-5C26F8FBF8E5', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 2, 4, '2026-04-06 08:42:00', '2026-04-05 12:00:00'),
('047BAD13-6409-4222-B347-80E839A6BC9A', '293E35A0-25B0-43B6-9574-709F14FE8564', '00CC925F-9671-4970-ADC6-5C26F8FBF8E5', N'Partial', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 3, 4, '2026-04-18 01:26:00', '2026-04-17 12:00:00'),
('6475044D-0965-4F6B-9B89-9CB5D9805DDC', '293E35A0-25B0-43B6-9574-709F14FE8564', '00CC925F-9671-4970-ADC6-5C26F8FBF8E5', N'Skipped', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 3, 3, '2026-04-28 22:16:00', '2026-04-28 12:00:00'),
('F44F961D-49C9-46E7-87B6-67E682EB6862', '293E35A0-25B0-43B6-9574-709F14FE8564', '00CC925F-9671-4970-ADC6-5C26F8FBF8E5', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 6, 8, '2026-05-10 20:31:00', '2026-05-10 12:00:00'),
('F0953088-766A-4209-B260-D3DD8BF8E6AE', '293E35A0-25B0-43B6-9574-709F14FE8564', '00CC925F-9671-4970-ADC6-5C26F8FBF8E5', N'Skipped', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 6, 5, '2026-05-21 23:05:00', '2026-05-21 12:00:00'),
('2FDA122F-6E12-4274-8908-1E058D18DBD4', '293E35A0-25B0-43B6-9574-709F14FE8564', '00CC925F-9671-4970-ADC6-5C26F8FBF8E5', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 6, 8, '2026-06-03 01:50:00', '2026-06-02 12:00:00'),
('FC43A1A6-283E-403B-AF95-B95D3597C19F', '293E35A0-25B0-43B6-9574-709F14FE8564', '00CC925F-9671-4970-ADC6-5C26F8FBF8E5', N'Skipped', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 5, 4, '2026-06-14 01:19:00', '2026-06-13 12:00:00'),
('D5A90B92-B91D-4973-8740-1B23D189CF2A', '4D335F31-F0BE-4982-9D4B-5CE701FF4127', '00CC925F-9671-4970-ADC6-5C26F8FBF8E5', N'Skipped', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 4, 4, '2026-04-11 03:15:00', '2026-04-10 12:00:00'),
('DC707516-25F3-4CB3-B4CB-3E977A4D911D', '4D335F31-F0BE-4982-9D4B-5CE701FF4127', '00CC925F-9671-4970-ADC6-5C26F8FBF8E5', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 3, 7, '2026-04-18 21:59:00', '2026-04-18 12:00:00'),
('AEDED0E2-675C-41C4-B179-99D2FD8DA347', '4D335F31-F0BE-4982-9D4B-5CE701FF4127', '00CC925F-9671-4970-ADC6-5C26F8FBF8E5', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 5, 9, '2026-04-27 05:06:00', '2026-04-26 12:00:00'),
('3D73F52A-0FF7-40B0-898B-F4D02E9D7ACF', '4D335F31-F0BE-4982-9D4B-5CE701FF4127', '00CC925F-9671-4970-ADC6-5C26F8FBF8E5', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 4, 7, '2026-05-05 01:12:00', '2026-05-04 12:00:00'),
('9F96F8F8-4D78-4882-872B-7D76D61E08AB', '4D335F31-F0BE-4982-9D4B-5CE701FF4127', '00CC925F-9671-4970-ADC6-5C26F8FBF8E5', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 7, 10, '2026-05-13 02:05:00', '2026-05-12 12:00:00'),
('4C95AC28-8615-433E-886B-4278672CF89A', 'A71FB1CE-D2CC-4DA3-AB76-2A00E5BE2BD6', '5947B1E9-EACB-4CD1-A014-69DBDA364362', N'Skipped', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 4, 3, '2025-12-16 06:52:00', '2025-12-15 12:00:00'),
('80F345B6-1E83-4555-AA30-DCC1A499737A', 'A71FB1CE-D2CC-4DA3-AB76-2A00E5BE2BD6', '5947B1E9-EACB-4CD1-A014-69DBDA364362', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 2, 5, '2025-12-24 07:54:00', '2025-12-23 12:00:00'),
('FCBCA7DA-F43B-47B5-8418-61344B33373A', 'A71FB1CE-D2CC-4DA3-AB76-2A00E5BE2BD6', '5947B1E9-EACB-4CD1-A014-69DBDA364362', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 3, 7, '2026-01-02 08:05:00', '2026-01-01 12:00:00'),
('4DE32F72-46B0-43EE-9E40-E2FA259DE77A', 'A71FB1CE-D2CC-4DA3-AB76-2A00E5BE2BD6', '5947B1E9-EACB-4CD1-A014-69DBDA364362', N'Partial', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 3, 4, '2026-01-09 21:38:00', '2026-01-09 12:00:00'),
('61659732-687A-41E3-860D-8A1A977AA730', 'A71FB1CE-D2CC-4DA3-AB76-2A00E5BE2BD6', '5947B1E9-EACB-4CD1-A014-69DBDA364362', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 6, 9, '2026-01-18 04:51:00', '2026-01-17 12:00:00'),
('4339AB1E-7546-4D5B-9AD4-CAEC3C2DA3E2', 'A71FB1CE-D2CC-4DA3-AB76-2A00E5BE2BD6', '5947B1E9-EACB-4CD1-A014-69DBDA364362', N'Partial', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 4, 4, '2026-01-26 05:49:00', '2026-01-25 12:00:00'),
('EE22E1AE-81CB-44F5-A29E-E3C0C12AEFD9', 'A71FB1CE-D2CC-4DA3-AB76-2A00E5BE2BD6', '5947B1E9-EACB-4CD1-A014-69DBDA364362', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 5, 9, '2026-02-03 21:19:00', '2026-02-03 12:00:00'),
('1F988AA1-BCA8-495B-A494-4AC15312E8B2', 'A71FB1CE-D2CC-4DA3-AB76-2A00E5BE2BD6', '5947B1E9-EACB-4CD1-A014-69DBDA364362', N'Skipped', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 7, 6, '2026-02-11 23:57:00', '2026-02-11 12:00:00'),
('D3263E3E-A097-484B-8476-03C49777EA35', 'A71FB1CE-D2CC-4DA3-AB76-2A00E5BE2BD6', '5947B1E9-EACB-4CD1-A014-69DBDA364362', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 6, 9, '2026-02-20 03:47:00', '2026-02-19 12:00:00'),
('041F012B-B43B-4788-9CAA-4436694254A6', '4DDC2FC3-5D35-4410-8FFC-B74F604D909D', '5947B1E9-EACB-4CD1-A014-69DBDA364362', N'Skipped', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 4, 4, '2025-12-11 23:45:00', '2025-12-11 12:00:00'),
('46000B1F-9004-41BA-B1A2-7A1489292E0C', '4DDC2FC3-5D35-4410-8FFC-B74F604D909D', '5947B1E9-EACB-4CD1-A014-69DBDA364362', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 2, 6, '2025-12-18 02:13:00', '2025-12-17 12:00:00'),
('038D2618-ACFB-42D4-A924-72790EB02CEF', '4DDC2FC3-5D35-4410-8FFC-B74F604D909D', '5947B1E9-EACB-4CD1-A014-69DBDA364362', N'Partial', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 3, 4, '2025-12-25 09:13:00', '2025-12-24 12:00:00'),
('6BF0CC27-5670-4E77-81F3-EDAC06030FB2', '4DDC2FC3-5D35-4410-8FFC-B74F604D909D', '5947B1E9-EACB-4CD1-A014-69DBDA364362', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 3, 5, '2025-12-31 05:40:00', '2025-12-30 12:00:00');
INSERT INTO [ExerciseLogs] ([Id], [ExerciseId], [PatientId], [CompletionStatus], [ReflectionNote], [MoodBefore], [MoodAfter], [LoggedAt], [CreatedAt])
VALUES
('B9CA7E4A-F7D0-4FF7-AB8D-C0849E015AEA', '4DDC2FC3-5D35-4410-8FFC-B74F604D909D', '5947B1E9-EACB-4CD1-A014-69DBDA364362', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 6, 8, '2026-01-06 04:19:00', '2026-01-05 12:00:00'),
('61AB0BC3-3E9F-4892-894F-546E6025AE8A', '4DDC2FC3-5D35-4410-8FFC-B74F604D909D', '5947B1E9-EACB-4CD1-A014-69DBDA364362', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 5, 7, '2026-01-12 07:24:00', '2026-01-11 12:00:00'),
('7E075407-483A-4650-BF30-7FDB8E198924', '4DDC2FC3-5D35-4410-8FFC-B74F604D909D', '5947B1E9-EACB-4CD1-A014-69DBDA364362', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 5, 8, '2026-01-19 00:39:00', '2026-01-18 12:00:00'),
('569977DD-0EB0-4FFB-B041-FFFA15F88C98', '4DDC2FC3-5D35-4410-8FFC-B74F604D909D', '5947B1E9-EACB-4CD1-A014-69DBDA364362', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 6, 9, '2026-01-24 20:22:00', '2026-01-24 12:00:00'),
('A3F8CBD4-BA2D-4A8B-8BC6-B54EBE69912F', '993F4F3F-FEBA-453B-9226-1F159001EC78', '5947B1E9-EACB-4CD1-A014-69DBDA364362', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 4, 6, '2025-12-14 07:48:00', '2025-12-13 12:00:00'),
('154E3186-527A-4A0D-9A3D-85CB14A74992', '993F4F3F-FEBA-453B-9226-1F159001EC78', '5947B1E9-EACB-4CD1-A014-69DBDA364362', N'Partial', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 3, 3, '2025-12-20 02:13:00', '2025-12-19 12:00:00'),
('F7BCF447-8491-435F-AF3A-0EF0CDC551FE', '993F4F3F-FEBA-453B-9226-1F159001EC78', '5947B1E9-EACB-4CD1-A014-69DBDA364362', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 4, 8, '2025-12-26 00:24:00', '2025-12-25 12:00:00'),
('869CD0F6-790B-4AE8-ABA6-042C87D1A960', '993F4F3F-FEBA-453B-9226-1F159001EC78', '5947B1E9-EACB-4CD1-A014-69DBDA364362', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 6, 9, '2026-01-01 04:10:00', '2025-12-31 12:00:00'),
('C8AE4114-9A7D-462E-BFF4-69478DA5B9C1', '993F4F3F-FEBA-453B-9226-1F159001EC78', '5947B1E9-EACB-4CD1-A014-69DBDA364362', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 4, 7, '2026-01-06 22:43:00', '2026-01-06 12:00:00'),
('5CFE478E-AF04-4B09-9904-C64503BEC0CF', '993F4F3F-FEBA-453B-9226-1F159001EC78', '5947B1E9-EACB-4CD1-A014-69DBDA364362', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 7, 9, '2026-01-13 04:47:00', '2026-01-12 12:00:00'),
('8035620B-774D-4F18-A40B-658EEA70DBEC', '5FBC3CF4-B008-4DC9-8AB1-1182FF5757EB', 'D05B32B7-0D94-4041-91CE-8952FF6C525A', N'Skipped', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 4, 3, '2025-07-26 08:07:00', '2025-07-25 12:00:00'),
('79A57E43-977D-4D9D-B1E4-7C2E3CF8486C', '5FBC3CF4-B008-4DC9-8AB1-1182FF5757EB', 'D05B32B7-0D94-4041-91CE-8952FF6C525A', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 5, 7, '2025-08-21 08:19:00', '2025-08-20 12:00:00'),
('60C44DF6-B40F-4AFE-A136-5EBE89EB0B38', '5FBC3CF4-B008-4DC9-8AB1-1182FF5757EB', 'D05B32B7-0D94-4041-91CE-8952FF6C525A', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 6, 8, '2025-09-15 22:02:00', '2025-09-15 12:00:00'),
('3E7347B8-DFCE-4347-8E37-E74E913BC83E', '5FBC3CF4-B008-4DC9-8AB1-1182FF5757EB', 'D05B32B7-0D94-4041-91CE-8952FF6C525A', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 6, 8, '2025-10-11 23:14:00', '2025-10-11 12:00:00'),
('96186A8C-5EB7-4395-AA83-58A2897CAECE', '23AA7606-5E11-4DDB-9DFB-DF597875D972', 'D05B32B7-0D94-4041-91CE-8952FF6C525A', N'Partial', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 2, 2, '2025-07-25 01:40:00', '2025-07-24 12:00:00'),
('DC509179-97D5-4F34-9820-357E5AF42852', '23AA7606-5E11-4DDB-9DFB-DF597875D972', 'D05B32B7-0D94-4041-91CE-8952FF6C525A', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 2, 6, '2025-08-01 22:10:00', '2025-08-01 12:00:00'),
('6B360958-4B2B-4D4A-B5FF-AB670AE4C2B1', '23AA7606-5E11-4DDB-9DFB-DF597875D972', 'D05B32B7-0D94-4041-91CE-8952FF6C525A', N'Partial', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 5, 5, '2025-08-11 04:31:00', '2025-08-10 12:00:00'),
('51599280-2BF0-472B-A68B-45B330B54B8C', '23AA7606-5E11-4DDB-9DFB-DF597875D972', 'D05B32B7-0D94-4041-91CE-8952FF6C525A', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 4, 7, '2025-08-19 05:56:00', '2025-08-18 12:00:00'),
('B10B0A9F-35E7-4DAA-922B-5351E0DAFC45', '23AA7606-5E11-4DDB-9DFB-DF597875D972', 'D05B32B7-0D94-4041-91CE-8952FF6C525A', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 4, 7, '2025-08-26 20:21:00', '2025-08-26 12:00:00'),
('9355E5CD-8407-406A-9122-44505619496A', '23AA7606-5E11-4DDB-9DFB-DF597875D972', 'D05B32B7-0D94-4041-91CE-8952FF6C525A', N'Partial', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 5, 7, '2025-09-04 02:34:00', '2025-09-03 12:00:00'),
('BBD42DB9-B6C3-4288-8694-AE3B7F130118', '23AA7606-5E11-4DDB-9DFB-DF597875D972', 'D05B32B7-0D94-4041-91CE-8952FF6C525A', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 4, 7, '2025-09-13 06:38:00', '2025-09-12 12:00:00'),
('66FE79F0-972A-44DB-8B98-07713D1F9DE1', '23AA7606-5E11-4DDB-9DFB-DF597875D972', 'D05B32B7-0D94-4041-91CE-8952FF6C525A', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 6, 9, '2025-09-21 07:35:00', '2025-09-20 12:00:00'),
('BEFE853C-9C15-4084-A42B-24E7D2DF6DCC', '23AA7606-5E11-4DDB-9DFB-DF597875D972', 'D05B32B7-0D94-4041-91CE-8952FF6C525A', N'Partial', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 6, 8, '2025-09-29 01:02:00', '2025-09-28 12:00:00'),
('7096BB12-BCC9-40B7-B945-A11A387EF394', '89455528-3F07-4F36-83C3-97D4A1596C91', '7F9C808B-66FB-44EC-AE2F-D987CCB0D98A', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 2, 6, '2026-03-01 23:11:00', '2026-03-01 12:00:00'),
('2F773964-1080-467D-AA66-8608378922E8', '89455528-3F07-4F36-83C3-97D4A1596C91', '7F9C808B-66FB-44EC-AE2F-D987CCB0D98A', N'Skipped', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 4, 4, '2026-03-06 22:26:00', '2026-03-06 12:00:00'),
('688D29A9-BF0F-472D-B3D1-16CB723B1239', '89455528-3F07-4F36-83C3-97D4A1596C91', '7F9C808B-66FB-44EC-AE2F-D987CCB0D98A', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 3, 5, '2026-03-11 23:12:00', '2026-03-11 12:00:00'),
('3CE0BDF0-FE2D-4BFC-868C-09744064211E', '89455528-3F07-4F36-83C3-97D4A1596C91', '7F9C808B-66FB-44EC-AE2F-D987CCB0D98A', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 4, 8, '2026-03-17 03:35:00', '2026-03-16 12:00:00'),
('74832FA4-1A25-4DA1-BFAB-AA8E72D978B5', '89455528-3F07-4F36-83C3-97D4A1596C91', '7F9C808B-66FB-44EC-AE2F-D987CCB0D98A', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 4, 6, '2026-03-21 20:20:00', '2026-03-21 12:00:00'),
('27004EB5-8AAA-4128-8BAA-4E39E24F8E1C', '89455528-3F07-4F36-83C3-97D4A1596C91', '7F9C808B-66FB-44EC-AE2F-D987CCB0D98A', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 6, 10, '2026-03-27 06:30:00', '2026-03-26 12:00:00'),
('CCBD6E32-B88F-453C-A5FF-222C32F167F4', '89455528-3F07-4F36-83C3-97D4A1596C91', '7F9C808B-66FB-44EC-AE2F-D987CCB0D98A', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 7, 10, '2026-04-01 09:04:00', '2026-03-31 12:00:00'),
('4F5E7718-7818-48CA-ACE3-3D1F504FDD54', '89455528-3F07-4F36-83C3-97D4A1596C91', '7F9C808B-66FB-44EC-AE2F-D987CCB0D98A', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 7, 9, '2026-04-06 08:39:00', '2026-04-05 12:00:00'),
('E474A4FC-600E-4070-A2D4-0EFFEBC8498E', 'C557AA9B-74D3-46BA-9A70-F4E87263ED95', '7F9C808B-66FB-44EC-AE2F-D987CCB0D98A', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 3, 7, '2026-02-26 21:31:00', '2026-02-26 12:00:00'),
('67C2A7D8-5DF6-4BE2-8A81-2C616347F574', 'C557AA9B-74D3-46BA-9A70-F4E87263ED95', '7F9C808B-66FB-44EC-AE2F-D987CCB0D98A', N'Partial', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 3, 3, '2026-03-12 20:42:00', '2026-03-12 12:00:00'),
('8C0900BA-6EFF-4B0B-B5F9-63CF90C57459', 'C557AA9B-74D3-46BA-9A70-F4E87263ED95', '7F9C808B-66FB-44EC-AE2F-D987CCB0D98A', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 5, 7, '2026-03-28 02:08:00', '2026-03-27 12:00:00'),
('C636CEA9-9A32-46D8-88DC-D181B8C1F96E', 'C557AA9B-74D3-46BA-9A70-F4E87263ED95', '7F9C808B-66FB-44EC-AE2F-D987CCB0D98A', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 6, 10, '2026-04-10 23:53:00', '2026-04-10 12:00:00'),
('2C965594-5AC0-4A31-9DB3-0E4AF5686819', 'C557AA9B-74D3-46BA-9A70-F4E87263ED95', '7F9C808B-66FB-44EC-AE2F-D987CCB0D98A', N'Partial', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 6, 7, '2026-04-26 06:45:00', '2026-04-25 12:00:00'),
('4E19C9F0-7207-4FE1-BEC4-6981B6CAE53E', 'C557AA9B-74D3-46BA-9A70-F4E87263ED95', '7F9C808B-66FB-44EC-AE2F-D987CCB0D98A', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 7, 10, '2026-05-10 04:23:00', '2026-05-09 12:00:00'),
('4F0CA814-D63E-443A-8630-DE3EE9F28750', '0E588A0A-0244-4BD6-BBC3-FBF133CB82A8', '7F9C808B-66FB-44EC-AE2F-D987CCB0D98A', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 2, 5, '2026-02-22 20:01:00', '2026-02-22 12:00:00'),
('41112529-DA40-4C6F-BA0E-CEC32D6EC247', '0E588A0A-0244-4BD6-BBC3-FBF133CB82A8', '7F9C808B-66FB-44EC-AE2F-D987CCB0D98A', N'Partial', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 5, 5, '2026-03-10 09:45:00', '2026-03-09 12:00:00'),
('453FA808-DB2B-40A9-8DF3-9F83C64A1776', '0E588A0A-0244-4BD6-BBC3-FBF133CB82A8', '7F9C808B-66FB-44EC-AE2F-D987CCB0D98A', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 3, 6, '2026-03-25 08:46:00', '2026-03-24 12:00:00'),
('2FA163AB-D46D-4B60-AF09-D3A35BF55883', '0E588A0A-0244-4BD6-BBC3-FBF133CB82A8', '7F9C808B-66FB-44EC-AE2F-D987CCB0D98A', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 4, 8, '2026-04-10 01:09:00', '2026-04-09 12:00:00'),
('8E80F20C-C972-4C90-B901-E0BC5406FEC2', '0E588A0A-0244-4BD6-BBC3-FBF133CB82A8', '7F9C808B-66FB-44EC-AE2F-D987CCB0D98A', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 4, 8, '2026-04-24 22:37:00', '2026-04-24 12:00:00'),
('2D043AED-4DE5-4609-BD0A-602E00C5313E', '0E588A0A-0244-4BD6-BBC3-FBF133CB82A8', '7F9C808B-66FB-44EC-AE2F-D987CCB0D98A', N'Skipped', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 5, 4, '2026-05-10 07:22:00', '2026-05-09 12:00:00'),
('495C22AA-984F-4EDC-94CB-70A617ABCAC6', 'E2F445FC-D55F-47B0-9E3C-0974847057C8', '080C5383-4556-405D-BBEC-38C843811EBD', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 3, 7, '2026-07-07 01:25:00', '2026-07-06 12:00:00'),
('B9B5DCC3-B37D-4941-A94A-4DA9F3B589E1', 'E2F445FC-D55F-47B0-9E3C-0974847057C8', '080C5383-4556-405D-BBEC-38C843811EBD', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 4, 8, '2026-07-07 07:28:00', '2026-07-06 12:00:00'),
('EFBF0FB9-3118-4FF4-8187-AEA233AACDD1', 'E2F445FC-D55F-47B0-9E3C-0974847057C8', '080C5383-4556-405D-BBEC-38C843811EBD', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 5, 7, '2026-07-07 00:44:00', '2026-07-06 12:00:00'),
('6D3BB853-DA25-41B5-87E3-69F2AC162FB2', 'E2F445FC-D55F-47B0-9E3C-0974847057C8', '080C5383-4556-405D-BBEC-38C843811EBD', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 5, 9, '2026-07-06 23:42:00', '2026-07-06 12:00:00'),
('79B00238-CC13-4F9F-AC72-56970AE395C0', '57D6AD95-1CD4-4057-BFEE-1C492E041297', '080C5383-4556-405D-BBEC-38C843811EBD', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 4, 8, '2026-07-05 20:49:00', '2026-07-05 12:00:00'),
('615E1BCC-8A53-4163-8451-7AF72B7A6982', '57D6AD95-1CD4-4057-BFEE-1C492E041297', '080C5383-4556-405D-BBEC-38C843811EBD', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 5, 7, '2026-07-05 23:49:00', '2026-07-05 12:00:00'),
('2561F827-16BD-4461-8959-698A8E75CDEE', '57D6AD95-1CD4-4057-BFEE-1C492E041297', '080C5383-4556-405D-BBEC-38C843811EBD', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 6, 8, '2026-07-07 03:37:00', '2026-07-06 12:00:00'),
('17EB643D-70A4-493F-8731-55BDA2CFAFBE', '57D6AD95-1CD4-4057-BFEE-1C492E041297', '080C5383-4556-405D-BBEC-38C843811EBD', N'Partial', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 7, 8, '2026-07-06 21:35:00', '2026-07-06 12:00:00'),
('900D2ADE-6AE1-4D03-B7B7-616BC245012F', '05E08E6C-2A93-4056-8768-6028A22C119A', '5A0DA59E-4AE0-4CF6-B7CD-5BDB2F91D5E7', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 2, 4, '2026-01-30 01:44:00', '2026-01-29 12:00:00'),
('04D0B939-7453-4A86-8A7F-47B2817BB709', '05E08E6C-2A93-4056-8768-6028A22C119A', '5A0DA59E-4AE0-4CF6-B7CD-5BDB2F91D5E7', N'Skipped', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 3, 2, '2026-02-16 04:21:00', '2026-02-15 12:00:00'),
('03955144-7849-4B5F-8EC0-FE724D4A2120', '05E08E6C-2A93-4056-8768-6028A22C119A', '5A0DA59E-4AE0-4CF6-B7CD-5BDB2F91D5E7', N'Partial', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 5, 6, '2026-03-05 21:16:00', '2026-03-05 12:00:00'),
('3D7D178E-1984-46BD-A969-9008045F07E3', '05E08E6C-2A93-4056-8768-6028A22C119A', '5A0DA59E-4AE0-4CF6-B7CD-5BDB2F91D5E7', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 5, 8, '2026-03-23 03:39:00', '2026-03-22 12:00:00'),
('8FB30946-6E22-47FA-8119-0764B19D9AF7', '05E08E6C-2A93-4056-8768-6028A22C119A', '5A0DA59E-4AE0-4CF6-B7CD-5BDB2F91D5E7', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 6, 10, '2026-04-09 01:31:00', '2026-04-08 12:00:00'),
('F0B8BE8D-FFE6-4833-8AB3-446B955105EA', '09D031EA-1776-420A-8C1B-EB00D86C3BF4', '5A0DA59E-4AE0-4CF6-B7CD-5BDB2F91D5E7', N'Partial', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 4, 6, '2026-02-01 23:19:00', '2026-02-01 12:00:00'),
('C3D61064-A016-414C-8D60-618EAB94AE57', '09D031EA-1776-420A-8C1B-EB00D86C3BF4', '5A0DA59E-4AE0-4CF6-B7CD-5BDB2F91D5E7', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 4, 6, '2026-02-16 21:40:00', '2026-02-16 12:00:00'),
('43C9D634-1F1D-4BA8-B8ED-4C1EE0523329', '09D031EA-1776-420A-8C1B-EB00D86C3BF4', '5A0DA59E-4AE0-4CF6-B7CD-5BDB2F91D5E7', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 5, 7, '2026-03-04 22:22:00', '2026-03-04 12:00:00'),
('F36931A4-C7A8-4589-923B-EB4E5DE82E0A', '09D031EA-1776-420A-8C1B-EB00D86C3BF4', '5A0DA59E-4AE0-4CF6-B7CD-5BDB2F91D5E7', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 4, 8, '2026-03-19 21:32:00', '2026-03-19 12:00:00'),
('4184A931-4DEE-4747-87A1-6927E216DEFA', '09D031EA-1776-420A-8C1B-EB00D86C3BF4', '5A0DA59E-4AE0-4CF6-B7CD-5BDB2F91D5E7', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 5, 7, '2026-04-04 01:42:00', '2026-04-03 12:00:00'),
('9A1A3A04-537B-41B3-ACA2-7E469D866B46', '6C48C212-2942-4A82-B5FB-ED315FCEE56E', '5A0DA59E-4AE0-4CF6-B7CD-5BDB2F91D5E7', N'Skipped', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 4, 3, '2026-02-06 06:49:00', '2026-02-05 12:00:00'),
('AA08F035-E1DC-481E-B6CA-A6678FEEEA08', '6C48C212-2942-4A82-B5FB-ED315FCEE56E', '5A0DA59E-4AE0-4CF6-B7CD-5BDB2F91D5E7', N'Partial', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 3, 3, '2026-02-13 09:43:00', '2026-02-12 12:00:00'),
('1DC38D09-D20E-4079-AD25-CF3B0B0D1C32', '6C48C212-2942-4A82-B5FB-ED315FCEE56E', '5A0DA59E-4AE0-4CF6-B7CD-5BDB2F91D5E7', N'Partial', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 5, 5, '2026-02-20 03:15:00', '2026-02-19 12:00:00'),
('6BF77B2F-1B63-4EF4-B40B-D0025A271B1F', '6C48C212-2942-4A82-B5FB-ED315FCEE56E', '5A0DA59E-4AE0-4CF6-B7CD-5BDB2F91D5E7', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 5, 8, '2026-02-26 20:19:00', '2026-02-26 12:00:00'),
('377CFD49-502D-40D4-BD08-CD925AAAE0D9', '6C48C212-2942-4A82-B5FB-ED315FCEE56E', '5A0DA59E-4AE0-4CF6-B7CD-5BDB2F91D5E7', N'Partial', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 5, 6, '2026-03-04 22:37:00', '2026-03-04 12:00:00'),
('5CF08139-178E-4F60-82A3-5F5AB6A36687', '6C48C212-2942-4A82-B5FB-ED315FCEE56E', '5A0DA59E-4AE0-4CF6-B7CD-5BDB2F91D5E7', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 5, 7, '2026-03-12 01:02:00', '2026-03-11 12:00:00'),
('513538F3-D885-47A8-9CD0-D6395EF20C5B', '6C48C212-2942-4A82-B5FB-ED315FCEE56E', '5A0DA59E-4AE0-4CF6-B7CD-5BDB2F91D5E7', N'Skipped', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 6, 7, '2026-03-19 03:59:00', '2026-03-18 12:00:00'),
('6CBCDE7A-54BD-4D24-B752-E3B3C5AC3DAB', '6C48C212-2942-4A82-B5FB-ED315FCEE56E', '5A0DA59E-4AE0-4CF6-B7CD-5BDB2F91D5E7', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 7, 10, '2026-03-26 00:10:00', '2026-03-25 12:00:00'),
('56D8E4D3-E7AB-4472-A86B-87F789D92CF8', '7E4618E7-E6EC-4C84-B1DD-4179999269D5', '0004E60D-C638-4639-BCFA-0AD58724C84E', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 4, 8, '2026-06-25 08:38:00', '2026-06-24 12:00:00'),
('C2646E5F-74A9-48B9-AABC-78C15997C26D', '7E4618E7-E6EC-4C84-B1DD-4179999269D5', '0004E60D-C638-4639-BCFA-0AD58724C84E', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 4, 7, '2026-06-28 04:44:00', '2026-06-27 12:00:00'),
('93121465-F23A-404E-9B44-71B950FDB88F', '7E4618E7-E6EC-4C84-B1DD-4179999269D5', '0004E60D-C638-4639-BCFA-0AD58724C84E', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 5, 9, '2026-07-01 04:09:00', '2026-06-30 12:00:00'),
('F014CF4A-8B3B-4574-9776-2F11E0ED22BA', '7E4618E7-E6EC-4C84-B1DD-4179999269D5', '0004E60D-C638-4639-BCFA-0AD58724C84E', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 4, 6, '2026-07-04 08:25:00', '2026-07-03 12:00:00'),
('42CCCEDF-E7F5-4E37-AA5C-7599E6BE5D8E', '7E4618E7-E6EC-4C84-B1DD-4179999269D5', '0004E60D-C638-4639-BCFA-0AD58724C84E', N'Skipped', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 5, 5, '2026-07-06 23:29:00', '2026-07-06 12:00:00'),
('8A9FD6CF-677A-416D-BEF6-873B91630067', '23FB3941-962F-4EAF-B853-24978618CB99', '0004E60D-C638-4639-BCFA-0AD58724C84E', N'Partial', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 2, 4, '2026-06-25 09:19:00', '2026-06-24 12:00:00'),
('C659DBEC-041E-4648-90B6-A7C11295944A', '23FB3941-962F-4EAF-B853-24978618CB99', '0004E60D-C638-4639-BCFA-0AD58724C84E', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 3, 6, '2026-06-28 20:42:00', '2026-06-28 12:00:00'),
('D37E1D25-5B94-4B7C-95C0-35020CE6E7D8', '23FB3941-962F-4EAF-B853-24978618CB99', '0004E60D-C638-4639-BCFA-0AD58724C84E', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 6, 8, '2026-07-02 20:29:00', '2026-07-02 12:00:00'),
('E9B35F08-4056-4042-A046-390578160DAD', '23FB3941-962F-4EAF-B853-24978618CB99', '0004E60D-C638-4639-BCFA-0AD58724C84E', N'Partial', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 6, 7, '2026-07-06 22:53:00', '2026-07-06 12:00:00'),
('2748C817-CB6A-48D3-BB90-8683FF3587AD', 'BFC9DD17-D408-4BA7-BA0D-3E088DCE86B0', '0004E60D-C638-4639-BCFA-0AD58724C84E', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 2, 6, '2026-06-24 01:16:00', '2026-06-23 12:00:00'),
('2A149238-AB4D-4C07-A7B7-414B75B82B2E', 'BFC9DD17-D408-4BA7-BA0D-3E088DCE86B0', '0004E60D-C638-4639-BCFA-0AD58724C84E', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 4, 8, '2026-06-28 03:35:00', '2026-06-27 12:00:00'),
('284B7764-F718-4FC9-93CE-6A9B8ED5A578', 'BFC9DD17-D408-4BA7-BA0D-3E088DCE86B0', '0004E60D-C638-4639-BCFA-0AD58724C84E', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 6, 10, '2026-07-02 21:45:00', '2026-07-02 12:00:00'),
('0EB92E76-87F4-4913-B902-324CC1C1DF6D', 'BFC9DD17-D408-4BA7-BA0D-3E088DCE86B0', '0004E60D-C638-4639-BCFA-0AD58724C84E', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 7, 10, '2026-07-07 08:36:00', '2026-07-06 12:00:00'),
('0053D428-AF07-461C-B1B9-EF2BB14D3CA9', '9AA8F5D7-D289-4398-B65D-ACAED2136578', '81B640AC-DBBD-4F29-831E-F36B5C6A7580', N'Partial', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 2, 3, '2026-03-13 00:35:00', '2026-03-12 12:00:00'),
('3DBDCFD8-4529-4692-B0A6-EC1AA497279B', '9AA8F5D7-D289-4398-B65D-ACAED2136578', '81B640AC-DBBD-4F29-831E-F36B5C6A7580', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 3, 6, '2026-03-23 02:08:00', '2026-03-22 12:00:00'),
('1E506754-4CF4-4BEF-BF6B-85A7C32493F1', '9AA8F5D7-D289-4398-B65D-ACAED2136578', '81B640AC-DBBD-4F29-831E-F36B5C6A7580', N'Skipped', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 5, 5, '2026-04-01 22:40:00', '2026-04-01 12:00:00'),
('ADDED3D0-9ABD-4E93-BCDA-1341D288FA2F', '9AA8F5D7-D289-4398-B65D-ACAED2136578', '81B640AC-DBBD-4F29-831E-F36B5C6A7580', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 4, 7, '2026-04-11 20:15:00', '2026-04-11 12:00:00'),
('F8E853B4-6F0B-4D79-A8C5-E3A656A8913B', '9AA8F5D7-D289-4398-B65D-ACAED2136578', '81B640AC-DBBD-4F29-831E-F36B5C6A7580', N'Partial', N'شعرت بهدوء أكبر بعد التمرين.', 4, 6, '2026-04-20 20:28:00', '2026-04-20 12:00:00'),
('1892039A-2205-487F-A4BD-1BF1D201CAB1', '9AA8F5D7-D289-4398-B65D-ACAED2136578', '81B640AC-DBBD-4F29-831E-F36B5C6A7580', N'Partial', N'شعرت بهدوء أكبر بعد التمرين.', 5, 6, '2026-04-30 22:27:00', '2026-04-30 12:00:00'),
('D4FFD1F1-72C1-4B8B-B61C-BE7D494C673B', '9AA8F5D7-D289-4398-B65D-ACAED2136578', '81B640AC-DBBD-4F29-831E-F36B5C6A7580', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 6, 9, '2026-05-11 01:14:00', '2026-05-10 12:00:00'),
('49C878B0-6689-4BE1-AC65-2B62F91DB60A', '9AA8F5D7-D289-4398-B65D-ACAED2136578', '81B640AC-DBBD-4F29-831E-F36B5C6A7580', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 6, 9, '2026-05-21 06:02:00', '2026-05-20 12:00:00'),
('07B1CE02-24DB-4AC8-916A-FCCBCF1A1792', 'EB2AAC9D-944F-4558-9F3C-FC0724E6F762', '81B640AC-DBBD-4F29-831E-F36B5C6A7580', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 4, 6, '2026-03-11 08:20:00', '2026-03-10 12:00:00'),
('5231240A-E82A-4EA4-B7C0-970516351581', 'EB2AAC9D-944F-4558-9F3C-FC0724E6F762', '81B640AC-DBBD-4F29-831E-F36B5C6A7580', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 4, 7, '2026-03-18 01:13:00', '2026-03-17 12:00:00'),
('C8489A3D-BEBB-4593-A3C5-CF7214FD27E9', 'EB2AAC9D-944F-4558-9F3C-FC0724E6F762', '81B640AC-DBBD-4F29-831E-F36B5C6A7580', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 3, 6, '2026-03-23 21:21:00', '2026-03-23 12:00:00'),
('7A603A1F-95B1-4D8F-8982-BFF747531083', 'EB2AAC9D-944F-4558-9F3C-FC0724E6F762', '81B640AC-DBBD-4F29-831E-F36B5C6A7580', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 3, 6, '2026-03-30 20:58:00', '2026-03-30 12:00:00'),
('EF6D567F-C2C6-41E3-BB62-861D0BE9BA41', 'EB2AAC9D-944F-4558-9F3C-FC0724E6F762', '81B640AC-DBBD-4F29-831E-F36B5C6A7580', N'Partial', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 5, 7, '2026-04-07 03:55:00', '2026-04-06 12:00:00'),
('7F98AA80-235D-4A31-9C04-D0B02CE6BBE1', 'EB2AAC9D-944F-4558-9F3C-FC0724E6F762', '81B640AC-DBBD-4F29-831E-F36B5C6A7580', N'Skipped', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 5, 5, '2026-04-13 21:22:00', '2026-04-13 12:00:00'),
('37C6E04E-CFB5-40F3-97BC-6D6DA4C97F3D', 'EB2AAC9D-944F-4558-9F3C-FC0724E6F762', '81B640AC-DBBD-4F29-831E-F36B5C6A7580', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 6, 10, '2026-04-20 03:35:00', '2026-04-19 12:00:00'),
('F1FF664E-EF04-4375-8943-4973E8B24AAD', 'EB2AAC9D-944F-4558-9F3C-FC0724E6F762', '81B640AC-DBBD-4F29-831E-F36B5C6A7580', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 5, 8, '2026-04-26 22:52:00', '2026-04-26 12:00:00'),
('730E6BE6-6DD7-463B-A1D3-466FA2A2C05C', 'B3E24B27-5A98-4EED-8350-E6AE24630879', '85C8A7BC-BBE9-4D03-B8D2-89AFEE304102', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 2, 5, '2026-02-19 05:27:00', '2026-02-18 12:00:00'),
('2F386EC7-D8C3-4564-A934-06F0D0338856', 'B3E24B27-5A98-4EED-8350-E6AE24630879', '85C8A7BC-BBE9-4D03-B8D2-89AFEE304102', N'Partial', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 5, 6, '2026-02-27 03:19:00', '2026-02-26 12:00:00'),
('C3F483FD-B22B-4543-B7BC-44688F497B9E', 'B3E24B27-5A98-4EED-8350-E6AE24630879', '85C8A7BC-BBE9-4D03-B8D2-89AFEE304102', N'Skipped', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 6, 5, '2026-03-06 08:32:00', '2026-03-05 12:00:00'),
('5BBC6D6A-4A66-4507-93AB-B0A4F06B024B', 'B3E24B27-5A98-4EED-8350-E6AE24630879', '85C8A7BC-BBE9-4D03-B8D2-89AFEE304102', N'Partial', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 5, 6, '2026-03-13 20:17:00', '2026-03-13 12:00:00'),
('7503E524-073A-474C-9EC3-157375E127F6', 'B3E24B27-5A98-4EED-8350-E6AE24630879', '85C8A7BC-BBE9-4D03-B8D2-89AFEE304102', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 5, 8, '2026-03-21 07:51:00', '2026-03-20 12:00:00'),
('603D8170-3686-4648-A059-844CB0793E81', '6572D047-F317-423C-9F84-A19D5D937565', '85C8A7BC-BBE9-4D03-B8D2-89AFEE304102', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 3, 6, '2026-02-12 23:40:00', '2026-02-12 12:00:00'),
('A4B3F1B3-4811-4666-A0FD-E5E43C219529', '6572D047-F317-423C-9F84-A19D5D937565', '85C8A7BC-BBE9-4D03-B8D2-89AFEE304102', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 5, 7, '2026-02-19 01:23:00', '2026-02-18 12:00:00'),
('DB7B6984-FF11-46DB-B0F0-EED2499AD8FE', '6572D047-F317-423C-9F84-A19D5D937565', '85C8A7BC-BBE9-4D03-B8D2-89AFEE304102', N'Partial', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 5, 7, '2026-02-24 22:28:00', '2026-02-24 12:00:00'),
('3843F461-716F-457E-BB72-998997CBEA8F', '6572D047-F317-423C-9F84-A19D5D937565', '85C8A7BC-BBE9-4D03-B8D2-89AFEE304102', N'Partial', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 4, 5, '2026-03-03 02:29:00', '2026-03-02 12:00:00'),
('68FBDB01-D4AF-4708-876E-D0AD1CF60593', '6572D047-F317-423C-9F84-A19D5D937565', '85C8A7BC-BBE9-4D03-B8D2-89AFEE304102', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 6, 8, '2026-03-09 00:09:00', '2026-03-08 12:00:00'),
('BE7764CC-AF82-4A61-B850-241762AE7E5B', '6572D047-F317-423C-9F84-A19D5D937565', '85C8A7BC-BBE9-4D03-B8D2-89AFEE304102', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 7, 10, '2026-03-14 22:53:00', '2026-03-14 12:00:00'),
('1CC3F8C6-022D-4D0E-BFA2-EAC68AB2163B', '6572D047-F317-423C-9F84-A19D5D937565', '85C8A7BC-BBE9-4D03-B8D2-89AFEE304102', N'Partial', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 7, 9, '2026-03-20 21:12:00', '2026-03-20 12:00:00'),
('A650F3FB-21A0-474D-821B-B36F11DFDB50', '6CD79544-698B-4BB9-8AC6-4604AEBC000B', '85C8A7BC-BBE9-4D03-B8D2-89AFEE304102', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 3, 6, '2026-02-13 23:31:00', '2026-02-13 12:00:00'),
('21AD4145-3373-4C08-8F5C-759B97DCBDEE', '6CD79544-698B-4BB9-8AC6-4604AEBC000B', '85C8A7BC-BBE9-4D03-B8D2-89AFEE304102', N'Partial', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 4, 6, '2026-02-20 03:23:00', '2026-02-19 12:00:00'),
('65BE7E49-AEE7-472E-AC83-0CED31BD9953', '6CD79544-698B-4BB9-8AC6-4604AEBC000B', '85C8A7BC-BBE9-4D03-B8D2-89AFEE304102', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 5, 8, '2026-02-24 20:27:00', '2026-02-24 12:00:00'),
('1D1E89B3-5E09-4DE1-BDCD-FCC461D28F29', '6CD79544-698B-4BB9-8AC6-4604AEBC000B', '85C8A7BC-BBE9-4D03-B8D2-89AFEE304102', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 4, 6, '2026-03-03 08:00:00', '2026-03-02 12:00:00'),
('E7D6C71D-038E-477E-AA4F-414746C3CBBC', '6CD79544-698B-4BB9-8AC6-4604AEBC000B', '85C8A7BC-BBE9-4D03-B8D2-89AFEE304102', N'Partial', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 4, 6, '2026-03-08 21:34:00', '2026-03-08 12:00:00'),
('EB9CAD90-E7AA-4BDC-9696-DD1137FB8C3B', '6CD79544-698B-4BB9-8AC6-4604AEBC000B', '85C8A7BC-BBE9-4D03-B8D2-89AFEE304102', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 6, 8, '2026-03-14 09:05:00', '2026-03-13 12:00:00'),
('5193EAAB-BEED-4D08-9D70-EA8153E52BE8', '6CD79544-698B-4BB9-8AC6-4604AEBC000B', '85C8A7BC-BBE9-4D03-B8D2-89AFEE304102', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 6, 8, '2026-03-20 05:31:00', '2026-03-19 12:00:00'),
('2FF65AF2-9E38-4BD5-805D-0C429C3743A1', '6CD79544-698B-4BB9-8AC6-4604AEBC000B', '85C8A7BC-BBE9-4D03-B8D2-89AFEE304102', N'Skipped', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 6, 5, '2026-03-25 01:57:00', '2026-03-24 12:00:00'),
('1E100C8C-FB17-44FC-9FB2-83ACDD66FAAA', '6CD79544-698B-4BB9-8AC6-4604AEBC000B', '85C8A7BC-BBE9-4D03-B8D2-89AFEE304102', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 5, 8, '2026-03-30 20:55:00', '2026-03-30 12:00:00'),
('D63CA775-D5EE-424A-812B-25531E29342A', '1832C622-71AD-44AE-ACD3-63A1AE2B8FCF', '46ACC094-18A3-4CCF-80C4-95D298EF4031', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 3, 6, '2026-02-28 01:51:00', '2026-02-27 12:00:00'),
('419910CB-F04A-4DE1-900F-B4793B4E37F4', '1832C622-71AD-44AE-ACD3-63A1AE2B8FCF', '46ACC094-18A3-4CCF-80C4-95D298EF4031', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 4, 7, '2026-03-14 21:11:00', '2026-03-14 12:00:00'),
('2D5B0811-2A21-4D8A-A3B2-597E583EBA3C', '1832C622-71AD-44AE-ACD3-63A1AE2B8FCF', '46ACC094-18A3-4CCF-80C4-95D298EF4031', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 5, 8, '2026-03-30 00:14:00', '2026-03-29 12:00:00'),
('DED69FF6-A92D-4899-96B0-BA29306D8D72', '1832C622-71AD-44AE-ACD3-63A1AE2B8FCF', '46ACC094-18A3-4CCF-80C4-95D298EF4031', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 4, 8, '2026-04-13 03:04:00', '2026-04-12 12:00:00'),
('BE578A21-2DCF-4340-8CE7-6A2CA34D431C', '1832C622-71AD-44AE-ACD3-63A1AE2B8FCF', '46ACC094-18A3-4CCF-80C4-95D298EF4031', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 6, 9, '2026-04-28 05:03:00', '2026-04-27 12:00:00'),
('3298D3F7-4FFA-4579-8754-7D97A48BEA9C', 'AD1CD297-85B1-4F97-B8B4-943BA34D87F7', '46ACC094-18A3-4CCF-80C4-95D298EF4031', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 4, 8, '2026-02-24 05:01:00', '2026-02-23 12:00:00'),
('E52A2698-078B-4FA4-8A34-CCFCB8CA7CAE', 'AD1CD297-85B1-4F97-B8B4-943BA34D87F7', '46ACC094-18A3-4CCF-80C4-95D298EF4031', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 3, 5, '2026-03-19 04:24:00', '2026-03-18 12:00:00'),
('5B0D53EA-680A-4117-B006-380E35A9CC5F', 'AD1CD297-85B1-4F97-B8B4-943BA34D87F7', '46ACC094-18A3-4CCF-80C4-95D298EF4031', N'Partial', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 4, 4, '2026-04-11 02:29:00', '2026-04-10 12:00:00'),
('2A685656-2D9D-4ACF-AED3-8ACEEDEA1AB4', 'AD1CD297-85B1-4F97-B8B4-943BA34D87F7', '46ACC094-18A3-4CCF-80C4-95D298EF4031', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 7, 9, '2026-05-04 00:57:00', '2026-05-03 12:00:00'),
('1511A1CB-8B07-4205-B150-720AC485F8F1', 'DE5290E1-489C-4A2D-B374-508E7B67BBE0', '46ACC094-18A3-4CCF-80C4-95D298EF4031', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 4, 6, '2026-02-28 23:39:00', '2026-02-28 12:00:00'),
('74BA34AB-1332-4370-A7E9-FE6BD8C0347A', 'DE5290E1-489C-4A2D-B374-508E7B67BBE0', '46ACC094-18A3-4CCF-80C4-95D298EF4031', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 4, 6, '2026-03-12 00:25:00', '2026-03-11 12:00:00'),
('4C2D3416-7223-490C-AB63-604F8D567B91', 'DE5290E1-489C-4A2D-B374-508E7B67BBE0', '46ACC094-18A3-4CCF-80C4-95D298EF4031', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 6, 8, '2026-03-23 05:41:00', '2026-03-22 12:00:00'),
('9BDBB767-2120-49C4-A9E5-3BF7538EE846', 'DE5290E1-489C-4A2D-B374-508E7B67BBE0', '46ACC094-18A3-4CCF-80C4-95D298EF4031', N'Skipped', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 5, 4, '2026-04-03 06:47:00', '2026-04-02 12:00:00'),
('E1EEB6CB-58F8-4F77-A9BE-065F9391FCE2', '996204F7-5016-4EA4-ABC7-2ED33C858D58', 'F300A791-B73E-4A86-90EA-4D1EE67BBE48', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 3, 7, '2026-06-05 20:35:00', '2026-06-05 12:00:00'),
('F4516B71-0C94-412C-A612-A49E70EDDF19', '996204F7-5016-4EA4-ABC7-2ED33C858D58', 'F300A791-B73E-4A86-90EA-4D1EE67BBE48', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 3, 7, '2026-06-11 22:07:00', '2026-06-11 12:00:00'),
('B5719DE1-A7E1-4BD5-B6D7-6B7726810F97', '996204F7-5016-4EA4-ABC7-2ED33C858D58', 'F300A791-B73E-4A86-90EA-4D1EE67BBE48', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 5, 8, '2026-06-17 21:34:00', '2026-06-17 12:00:00'),
('453887A6-FBB9-45F7-9C02-5C6CCF4892EF', '996204F7-5016-4EA4-ABC7-2ED33C858D58', 'F300A791-B73E-4A86-90EA-4D1EE67BBE48', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 5, 8, '2026-06-24 22:59:00', '2026-06-24 12:00:00'),
('316B4303-42D7-427C-8FF6-7AF7B397F89D', '996204F7-5016-4EA4-ABC7-2ED33C858D58', 'F300A791-B73E-4A86-90EA-4D1EE67BBE48', N'Partial', N'شعرت بهدوء أكبر بعد التمرين.', 4, 4, '2026-07-01 00:05:00', '2026-06-30 12:00:00'),
('3DD74823-A908-4E5E-89CA-54F24F052B4B', '996204F7-5016-4EA4-ABC7-2ED33C858D58', 'F300A791-B73E-4A86-90EA-4D1EE67BBE48', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 7, 9, '2026-07-07 07:03:00', '2026-07-06 12:00:00'),
('80D5E04E-8C26-4892-9316-0435F7C39DF4', 'C5FC3C51-7F40-4B7B-81D5-5BAB80739E81', 'F300A791-B73E-4A86-90EA-4D1EE67BBE48', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 2, 5, '2026-06-12 03:29:00', '2026-06-11 12:00:00'),
('1D915C97-29AB-4E6C-BE4D-C30510177564', 'C5FC3C51-7F40-4B7B-81D5-5BAB80739E81', 'F300A791-B73E-4A86-90EA-4D1EE67BBE48', N'Partial', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 3, 4, '2026-06-17 22:35:00', '2026-06-17 12:00:00'),
('FB0A804E-711E-473F-83F6-A07FC68DD8C7', 'C5FC3C51-7F40-4B7B-81D5-5BAB80739E81', 'F300A791-B73E-4A86-90EA-4D1EE67BBE48', N'Partial', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 5, 7, '2026-06-25 02:28:00', '2026-06-24 12:00:00'),
('AB1417C6-D413-4ADE-AB35-40F88BB4208D', 'C5FC3C51-7F40-4B7B-81D5-5BAB80739E81', 'F300A791-B73E-4A86-90EA-4D1EE67BBE48', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 5, 9, '2026-07-01 01:23:00', '2026-06-30 12:00:00'),
('87D71773-F0E1-40D8-B4CE-5B099E03744C', 'C5FC3C51-7F40-4B7B-81D5-5BAB80739E81', 'F300A791-B73E-4A86-90EA-4D1EE67BBE48', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 6, 8, '2026-07-06 20:26:00', '2026-07-06 12:00:00'),
('F8A47852-893B-4272-9860-495775E8222A', 'ECDD0990-1553-4F55-AFA4-4BF09AB009ED', 'F300A791-B73E-4A86-90EA-4D1EE67BBE48', N'Partial', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 4, 6, '2026-06-07 07:21:00', '2026-06-06 12:00:00'),
('3845743C-F386-46FF-A665-DF2BD819E6E4', 'ECDD0990-1553-4F55-AFA4-4BF09AB009ED', 'F300A791-B73E-4A86-90EA-4D1EE67BBE48', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 4, 6, '2026-06-14 21:26:00', '2026-06-14 12:00:00'),
('3805E058-888A-42F9-91A4-80DA92856908', 'ECDD0990-1553-4F55-AFA4-4BF09AB009ED', 'F300A791-B73E-4A86-90EA-4D1EE67BBE48', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 6, 8, '2026-06-22 09:59:00', '2026-06-21 12:00:00'),
('5713185F-CA8E-4AE7-883B-A61478958635', 'ECDD0990-1553-4F55-AFA4-4BF09AB009ED', 'F300A791-B73E-4A86-90EA-4D1EE67BBE48', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 4, 6, '2026-06-30 06:43:00', '2026-06-29 12:00:00'),
('EAEE4DD6-73AF-45D8-A069-BD5732FD5C56', 'ECDD0990-1553-4F55-AFA4-4BF09AB009ED', 'F300A791-B73E-4A86-90EA-4D1EE67BBE48', N'Skipped', N'شعرت بهدوء أكبر بعد التمرين.', 5, 5, '2026-07-06 20:02:00', '2026-07-06 12:00:00'),
('D2DA1114-4842-4321-AE16-31A48DF19376', '0196B6A9-6360-40A4-99DC-E286F7BEB01D', 'F300A791-B73E-4A86-90EA-4D1EE67BBE48', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 4, 8, '2026-06-06 04:59:00', '2026-06-05 12:00:00'),
('95ED6657-B9BB-4E17-BE12-35A106EC4395', '0196B6A9-6360-40A4-99DC-E286F7BEB01D', 'F300A791-B73E-4A86-90EA-4D1EE67BBE48', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 2, 4, '2026-06-10 00:05:00', '2026-06-09 12:00:00'),
('C293C3F8-7A2D-4387-A4FD-B4024465E774', '0196B6A9-6360-40A4-99DC-E286F7BEB01D', 'F300A791-B73E-4A86-90EA-4D1EE67BBE48', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 5, 7, '2026-06-15 01:00:00', '2026-06-14 12:00:00'),
('F2E63313-B30A-4C63-A56A-3676F8CA6132', '0196B6A9-6360-40A4-99DC-E286F7BEB01D', 'F300A791-B73E-4A86-90EA-4D1EE67BBE48', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 3, 7, '2026-06-18 21:19:00', '2026-06-18 12:00:00'),
('CC4866E9-E9C8-483F-B722-DB686A9D599E', '0196B6A9-6360-40A4-99DC-E286F7BEB01D', 'F300A791-B73E-4A86-90EA-4D1EE67BBE48', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 6, 9, '2026-06-24 02:13:00', '2026-06-23 12:00:00'),
('B5FEC687-91FC-4A04-989B-CD6591DC2BA6', '0196B6A9-6360-40A4-99DC-E286F7BEB01D', 'F300A791-B73E-4A86-90EA-4D1EE67BBE48', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 4, 8, '2026-06-28 07:02:00', '2026-06-27 12:00:00'),
('96C016BA-1D5A-41E4-9BEB-3A81112D7D60', '0196B6A9-6360-40A4-99DC-E286F7BEB01D', 'F300A791-B73E-4A86-90EA-4D1EE67BBE48', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 5, 8, '2026-07-03 05:11:00', '2026-07-02 12:00:00'),
('5F443874-4207-4BAC-B532-46AC066552E2', '0196B6A9-6360-40A4-99DC-E286F7BEB01D', 'F300A791-B73E-4A86-90EA-4D1EE67BBE48', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 6, 9, '2026-07-06 21:18:00', '2026-07-06 12:00:00'),
('649B3062-038E-4B4A-924F-5DD4875E821E', '8F19F2C4-4B41-40F6-B508-EF9B0298AD82', '74FDAC62-9C1A-472B-AA93-BCDC4103514D', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 2, 5, '2026-01-25 04:21:00', '2026-01-24 12:00:00'),
('CD61120D-F17E-4A90-BAFC-60C3A4F4E292', '8F19F2C4-4B41-40F6-B508-EF9B0298AD82', '74FDAC62-9C1A-472B-AA93-BCDC4103514D', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 3, 5, '2026-02-17 21:19:00', '2026-02-17 12:00:00'),
('6E94B1C0-D126-4B03-98E7-8D4559D2FC6C', '8F19F2C4-4B41-40F6-B508-EF9B0298AD82', '74FDAC62-9C1A-472B-AA93-BCDC4103514D', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 5, 8, '2026-03-14 08:08:00', '2026-03-13 12:00:00'),
('52790965-DD05-4437-B68E-44457A2755F7', '8F19F2C4-4B41-40F6-B508-EF9B0298AD82', '74FDAC62-9C1A-472B-AA93-BCDC4103514D', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 7, 10, '2026-04-07 00:47:00', '2026-04-06 12:00:00'),
('6EBF6010-7125-4899-ACE9-AC20131222ED', 'E6A9D146-0B6F-4F18-857F-75F0AB47A87D', '74FDAC62-9C1A-472B-AA93-BCDC4103514D', N'Partial', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 2, 3, '2026-01-26 22:08:00', '2026-01-26 12:00:00'),
('68CB0B40-219E-4966-BBEA-1E0BC050B502', 'E6A9D146-0B6F-4F18-857F-75F0AB47A87D', '74FDAC62-9C1A-472B-AA93-BCDC4103514D', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 4, 6, '2026-02-03 08:00:00', '2026-02-02 12:00:00'),
('4FF1623D-9314-4560-82E5-803ABE3B4087', 'E6A9D146-0B6F-4F18-857F-75F0AB47A87D', '74FDAC62-9C1A-472B-AA93-BCDC4103514D', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 5, 7, '2026-02-09 21:35:00', '2026-02-09 12:00:00'),
('1FF8DC0D-5682-40D0-AC0B-4F321A6A24D4', 'E6A9D146-0B6F-4F18-857F-75F0AB47A87D', '74FDAC62-9C1A-472B-AA93-BCDC4103514D', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 4, 6, '2026-02-17 00:59:00', '2026-02-16 12:00:00'),
('8991A545-7FFD-4956-AB4C-39BC6DD4BA6C', 'E6A9D146-0B6F-4F18-857F-75F0AB47A87D', '74FDAC62-9C1A-472B-AA93-BCDC4103514D', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 6, 10, '2026-02-24 00:08:00', '2026-02-23 12:00:00'),
('3A97EE24-1809-4334-9281-1D9E88809DB1', 'E6A9D146-0B6F-4F18-857F-75F0AB47A87D', '74FDAC62-9C1A-472B-AA93-BCDC4103514D', N'Skipped', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 5, 6, '2026-03-02 03:25:00', '2026-03-01 12:00:00'),
('B6024B76-ADC9-4CCF-A057-D9A54EEFF981', 'E6A9D146-0B6F-4F18-857F-75F0AB47A87D', '74FDAC62-9C1A-472B-AA93-BCDC4103514D', N'Partial', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 5, 7, '2026-03-09 09:39:00', '2026-03-08 12:00:00'),
('F2E3FC86-76A2-449A-B93A-0DA40011CC32', 'E6A9D146-0B6F-4F18-857F-75F0AB47A87D', '74FDAC62-9C1A-472B-AA93-BCDC4103514D', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 7, 9, '2026-03-16 02:31:00', '2026-03-15 12:00:00'),
('F544B89D-126F-43F5-9AB9-60A609651BFA', 'E6A9D146-0B6F-4F18-857F-75F0AB47A87D', '74FDAC62-9C1A-472B-AA93-BCDC4103514D', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 6, 9, '2026-03-23 07:39:00', '2026-03-22 12:00:00'),
('F891330B-A00E-4D22-982C-D4E9EB7EC7F2', '8CE11432-F192-4F06-AF8E-E16AC00D0CDC', '5A2EBBD8-21B9-47E7-881C-B09F16F70FE6', N'Partial', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 3, 5, '2026-06-07 04:20:00', '2026-06-06 12:00:00'),
('AB24C2C3-19D1-462A-AFC8-6CA1F43F5853', '8CE11432-F192-4F06-AF8E-E16AC00D0CDC', '5A2EBBD8-21B9-47E7-881C-B09F16F70FE6', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 4, 6, '2026-06-12 04:31:00', '2026-06-11 12:00:00'),
('EBD233B2-661E-4055-B8CB-9823E31AE522', '8CE11432-F192-4F06-AF8E-E16AC00D0CDC', '5A2EBBD8-21B9-47E7-881C-B09F16F70FE6', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 3, 5, '2026-06-17 09:56:00', '2026-06-16 12:00:00'),
('CE39DCE5-2368-4FE9-9E53-4417248A02E1', '8CE11432-F192-4F06-AF8E-E16AC00D0CDC', '5A2EBBD8-21B9-47E7-881C-B09F16F70FE6', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 5, 9, '2026-06-22 02:18:00', '2026-06-21 12:00:00'),
('C24B5545-BF09-4336-8536-6E237E057EF1', '8CE11432-F192-4F06-AF8E-E16AC00D0CDC', '5A2EBBD8-21B9-47E7-881C-B09F16F70FE6', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 4, 7, '2026-06-26 22:45:00', '2026-06-26 12:00:00'),
('33A6B022-2255-4F5C-9098-F22F1037F105', '8CE11432-F192-4F06-AF8E-E16AC00D0CDC', '5A2EBBD8-21B9-47E7-881C-B09F16F70FE6', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 5, 8, '2026-07-02 05:55:00', '2026-07-01 12:00:00'),
('EFB6B3DB-FCCC-4B7C-8D0C-AEBAFB0CCE5F', '8CE11432-F192-4F06-AF8E-E16AC00D0CDC', '5A2EBBD8-21B9-47E7-881C-B09F16F70FE6', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 7, 9, '2026-07-07 08:00:00', '2026-07-06 12:00:00'),
('550974A4-6ED8-4060-8C1B-0CFB72BDD311', '49B2FED3-64E9-496A-A2F4-3339277814E4', '5A2EBBD8-21B9-47E7-881C-B09F16F70FE6', N'Partial', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 2, 3, '2026-06-09 06:35:00', '2026-06-08 12:00:00'),
('67C9D794-6846-471F-B2C7-1C6014851BFE', '49B2FED3-64E9-496A-A2F4-3339277814E4', '5A2EBBD8-21B9-47E7-881C-B09F16F70FE6', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 3, 5, '2026-06-14 05:13:00', '2026-06-13 12:00:00'),
('30D86360-6991-414B-9DD4-8A322ED1AB73', '49B2FED3-64E9-496A-A2F4-3339277814E4', '5A2EBBD8-21B9-47E7-881C-B09F16F70FE6', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 3, 6, '2026-06-17 23:29:00', '2026-06-17 12:00:00'),
('30CE7933-7DE8-42DC-9A9F-79B2A353F40C', '49B2FED3-64E9-496A-A2F4-3339277814E4', '5A2EBBD8-21B9-47E7-881C-B09F16F70FE6', N'Partial', N'شعرت بهدوء أكبر بعد التمرين.', 4, 6, '2026-06-23 00:06:00', '2026-06-22 12:00:00'),
('A60F9A88-D192-4109-992C-A4331DBB3A91', '49B2FED3-64E9-496A-A2F4-3339277814E4', '5A2EBBD8-21B9-47E7-881C-B09F16F70FE6', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 5, 8, '2026-06-28 08:43:00', '2026-06-27 12:00:00'),
('BF548C58-71CD-454B-AF55-C1466181F0DF', '49B2FED3-64E9-496A-A2F4-3339277814E4', '5A2EBBD8-21B9-47E7-881C-B09F16F70FE6', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 5, 7, '2026-07-02 00:41:00', '2026-07-01 12:00:00'),
('13869E6D-DE0C-4302-B0A1-61FBFC0D292B', '49B2FED3-64E9-496A-A2F4-3339277814E4', '5A2EBBD8-21B9-47E7-881C-B09F16F70FE6', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 5, 7, '2026-07-07 04:17:00', '2026-07-06 12:00:00'),
('81AD106F-8E0F-40EC-BC95-F731276F9BC4', 'B253C21C-01F6-478E-8343-ACC9F40815DB', '5A2EBBD8-21B9-47E7-881C-B09F16F70FE6', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 3, 6, '2026-06-12 04:26:00', '2026-06-11 12:00:00'),
('352199DB-9BDB-45BB-AE52-2D7AEAB5C3D8', 'B253C21C-01F6-478E-8343-ACC9F40815DB', '5A2EBBD8-21B9-47E7-881C-B09F16F70FE6', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 4, 7, '2026-06-14 23:47:00', '2026-06-14 12:00:00'),
('AB4B62F1-7045-4220-B221-544199B1A085', 'B253C21C-01F6-478E-8343-ACC9F40815DB', '5A2EBBD8-21B9-47E7-881C-B09F16F70FE6', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 4, 6, '2026-06-17 23:22:00', '2026-06-17 12:00:00'),
('706147D3-7495-4D05-8A9B-E19E1CC56501', 'B253C21C-01F6-478E-8343-ACC9F40815DB', '5A2EBBD8-21B9-47E7-881C-B09F16F70FE6', N'Partial', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 5, 7, '2026-06-21 01:02:00', '2026-06-20 12:00:00'),
('DC7751A7-7A53-40A7-BBC7-27FD649C1D33', 'B253C21C-01F6-478E-8343-ACC9F40815DB', '5A2EBBD8-21B9-47E7-881C-B09F16F70FE6', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 5, 9, '2026-06-24 22:43:00', '2026-06-24 12:00:00'),
('B84B680D-C316-4FAC-91A8-90BA54B331B6', 'B253C21C-01F6-478E-8343-ACC9F40815DB', '5A2EBBD8-21B9-47E7-881C-B09F16F70FE6', N'Skipped', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 5, 5, '2026-06-27 23:08:00', '2026-06-27 12:00:00'),
('B562E6D0-99AE-454B-A8BA-591DAD57878D', 'B253C21C-01F6-478E-8343-ACC9F40815DB', '5A2EBBD8-21B9-47E7-881C-B09F16F70FE6', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 6, 9, '2026-06-30 22:48:00', '2026-06-30 12:00:00'),
('5CD90BD4-D766-4058-B2A1-995946260693', 'B253C21C-01F6-478E-8343-ACC9F40815DB', '5A2EBBD8-21B9-47E7-881C-B09F16F70FE6', N'Partial', N'شعرت بهدوء أكبر بعد التمرين.', 5, 5, '2026-07-03 22:36:00', '2026-07-03 12:00:00'),
('51271032-7BEF-4C94-9FD4-71908261CD81', 'B253C21C-01F6-478E-8343-ACC9F40815DB', '5A2EBBD8-21B9-47E7-881C-B09F16F70FE6', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 5, 8, '2026-07-06 21:00:00', '2026-07-06 12:00:00'),
('E1A364E3-44E3-4EAC-9BCF-E31F29289648', 'EA3C0CD0-E803-4F90-99D4-D1BB22690EA6', '5A2EBBD8-21B9-47E7-881C-B09F16F70FE6', N'Skipped', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 4, 4, '2026-06-13 03:22:00', '2026-06-12 12:00:00'),
('483F8BEB-2A9B-46B8-AAA4-225985C0CB72', 'EA3C0CD0-E803-4F90-99D4-D1BB22690EA6', '5A2EBBD8-21B9-47E7-881C-B09F16F70FE6', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 2, 6, '2026-06-16 02:52:00', '2026-06-15 12:00:00'),
('DCF92E2C-91F6-4496-9AC7-ED417CB814A7', 'EA3C0CD0-E803-4F90-99D4-D1BB22690EA6', '5A2EBBD8-21B9-47E7-881C-B09F16F70FE6', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 4, 6, '2026-06-20 09:03:00', '2026-06-19 12:00:00'),
('69265B81-8A83-4D10-A582-5208A5948BF4', 'EA3C0CD0-E803-4F90-99D4-D1BB22690EA6', '5A2EBBD8-21B9-47E7-881C-B09F16F70FE6', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 5, 7, '2026-06-23 06:04:00', '2026-06-22 12:00:00'),
('13258B5E-37B8-41BC-B781-A96CCCE9765A', 'EA3C0CD0-E803-4F90-99D4-D1BB22690EA6', '5A2EBBD8-21B9-47E7-881C-B09F16F70FE6', N'Skipped', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 4, 4, '2026-06-26 22:30:00', '2026-06-26 12:00:00'),
('BD68E47C-2DB5-4C6C-9BDF-AC415AB83FD5', 'EA3C0CD0-E803-4F90-99D4-D1BB22690EA6', '5A2EBBD8-21B9-47E7-881C-B09F16F70FE6', N'Skipped', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 6, 7, '2026-06-30 02:43:00', '2026-06-29 12:00:00'),
('559A7BCE-ABA0-4CD6-9320-4621296DABA8', 'EA3C0CD0-E803-4F90-99D4-D1BB22690EA6', '5A2EBBD8-21B9-47E7-881C-B09F16F70FE6', N'Partial', N'شعرت بهدوء أكبر بعد التمرين.', 5, 7, '2026-07-03 20:19:00', '2026-07-03 12:00:00'),
('6D696823-AE45-4088-B328-1AA738CDE936', 'EA3C0CD0-E803-4F90-99D4-D1BB22690EA6', '5A2EBBD8-21B9-47E7-881C-B09F16F70FE6', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 7, 9, '2026-07-07 07:54:00', '2026-07-06 12:00:00'),
('F5B71AE6-CC64-46F7-977B-4C414513626E', '7F6DBE59-0763-440F-9A4A-467F6A1970BD', 'CC37E596-7078-4E46-938E-D6B59CA67B02', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 4, 7, '2026-03-21 00:38:00', '2026-03-20 12:00:00'),
('28324608-6346-4240-850F-44C099906697', '7F6DBE59-0763-440F-9A4A-467F6A1970BD', 'CC37E596-7078-4E46-938E-D6B59CA67B02', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 5, 7, '2026-04-04 04:05:00', '2026-04-03 12:00:00'),
('823FAFE4-C374-4BA8-9695-F7FFAC7162F9', '7F6DBE59-0763-440F-9A4A-467F6A1970BD', 'CC37E596-7078-4E46-938E-D6B59CA67B02', N'Skipped', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 6, 7, '2026-04-18 00:10:00', '2026-04-17 12:00:00'),
('AC137C76-1F52-47E0-B85C-CB0A3546422E', '7F6DBE59-0763-440F-9A4A-467F6A1970BD', 'CC37E596-7078-4E46-938E-D6B59CA67B02', N'Partial', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 6, 7, '2026-05-01 01:50:00', '2026-04-30 12:00:00'),
('43ADB7F9-7B05-40D5-89FE-2F618D0FA924', '7F6DBE59-0763-440F-9A4A-467F6A1970BD', 'CC37E596-7078-4E46-938E-D6B59CA67B02', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 7, 10, '2026-05-15 07:45:00', '2026-05-14 12:00:00'),
('00B42DB7-BA8E-4636-9F34-7CABA82FF88A', '8FF7B388-D36B-4D71-935E-C325F10B317C', 'CC37E596-7078-4E46-938E-D6B59CA67B02', N'Partial', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 2, 3, '2026-03-21 05:11:00', '2026-03-20 12:00:00'),
('34831FEF-3781-4D76-8C2C-69C28C7BF493', '8FF7B388-D36B-4D71-935E-C325F10B317C', 'CC37E596-7078-4E46-938E-D6B59CA67B02', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 3, 5, '2026-03-29 06:36:00', '2026-03-28 12:00:00'),
('CF15EEC1-8281-4A04-952B-8F7A3341EDF4', '8FF7B388-D36B-4D71-935E-C325F10B317C', 'CC37E596-7078-4E46-938E-D6B59CA67B02', N'Skipped', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 4, 5, '2026-04-05 01:50:00', '2026-04-04 12:00:00'),
('A086AD58-62BF-4526-B49A-0AFF3990BB31', '8FF7B388-D36B-4D71-935E-C325F10B317C', 'CC37E596-7078-4E46-938E-D6B59CA67B02', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 4, 7, '2026-04-12 23:39:00', '2026-04-12 12:00:00'),
('A6A2F24E-8BCB-4131-8AC2-AC47EDC22F93', '8FF7B388-D36B-4D71-935E-C325F10B317C', 'CC37E596-7078-4E46-938E-D6B59CA67B02', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 6, 9, '2026-04-20 01:51:00', '2026-04-19 12:00:00'),
('17C4E919-2202-4D67-BCB6-3955F01BF82C', 'DF3507EA-8CE2-4E5A-9CE2-43B4BBBFF4CD', 'CC37E596-7078-4E46-938E-D6B59CA67B02', N'Partial', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 3, 4, '2026-03-16 22:26:00', '2026-03-16 12:00:00'),
('405F4562-B1A7-49A3-9F18-5BE6F32C8BB8', 'DF3507EA-8CE2-4E5A-9CE2-43B4BBBFF4CD', 'CC37E596-7078-4E46-938E-D6B59CA67B02', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 3, 5, '2026-03-27 07:03:00', '2026-03-26 12:00:00'),
('1D9DC17F-F8D3-4A4A-8C97-5544AF5F9E86', 'DF3507EA-8CE2-4E5A-9CE2-43B4BBBFF4CD', 'CC37E596-7078-4E46-938E-D6B59CA67B02', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 6, 8, '2026-04-05 20:03:00', '2026-04-05 12:00:00'),
('11777673-D5B8-4C6B-88B0-D34F0139EA90', 'DF3507EA-8CE2-4E5A-9CE2-43B4BBBFF4CD', 'CC37E596-7078-4E46-938E-D6B59CA67B02', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 5, 7, '2026-04-16 07:28:00', '2026-04-15 12:00:00'),
('61A2408C-70CC-4DEC-AD96-30AF5684B4E9', '638A5E2A-D953-4F46-B5F1-B8F1CCB23F67', 'CC37E596-7078-4E46-938E-D6B59CA67B02', N'Partial', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 3, 4, '2026-03-20 04:32:00', '2026-03-19 12:00:00'),
('A565A1EB-02DB-4546-88BD-4CE0D481CD8F', '638A5E2A-D953-4F46-B5F1-B8F1CCB23F67', 'CC37E596-7078-4E46-938E-D6B59CA67B02', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 5, 7, '2026-04-03 03:01:00', '2026-04-02 12:00:00'),
('6CED940F-07B6-45E1-A27D-986F6E1ED1CB', '638A5E2A-D953-4F46-B5F1-B8F1CCB23F67', 'CC37E596-7078-4E46-938E-D6B59CA67B02', N'Skipped', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 6, 6, '2026-04-17 06:48:00', '2026-04-16 12:00:00'),
('87A00BA8-5BA6-4B7A-8D6A-34B5535A674A', '638A5E2A-D953-4F46-B5F1-B8F1CCB23F67', 'CC37E596-7078-4E46-938E-D6B59CA67B02', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 6, 8, '2026-04-30 00:46:00', '2026-04-29 12:00:00'),
('D3434FB6-9F1B-445A-8949-FC3F36446797', '638A5E2A-D953-4F46-B5F1-B8F1CCB23F67', 'CC37E596-7078-4E46-938E-D6B59CA67B02', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 7, 9, '2026-05-13 20:08:00', '2026-05-13 12:00:00'),
('747FC83E-68AD-42F6-8BE7-441D070550C7', 'C07977DB-A6B5-432F-8A47-D0566310210A', '5F6B076C-526F-4F65-B5E3-F416520D1841', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 2, 4, '2026-03-16 06:43:00', '2026-03-15 12:00:00'),
('E27785CE-24E4-411F-BC41-30B073757E2C', 'C07977DB-A6B5-432F-8A47-D0566310210A', '5F6B076C-526F-4F65-B5E3-F416520D1841', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 4, 6, '2026-03-27 23:27:00', '2026-03-27 12:00:00'),
('A235A133-BC58-4601-BBFB-C0B4D8608364', 'C07977DB-A6B5-432F-8A47-D0566310210A', '5F6B076C-526F-4F65-B5E3-F416520D1841', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 3, 5, '2026-04-09 08:42:00', '2026-04-08 12:00:00'),
('8DD916B3-D602-4153-BDE3-22E6E2D3291E', 'C07977DB-A6B5-432F-8A47-D0566310210A', '5F6B076C-526F-4F65-B5E3-F416520D1841', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 4, 7, '2026-04-21 08:46:00', '2026-04-20 12:00:00'),
('1BDFB945-D4B7-43A6-A084-85D457B7DFEE', 'C07977DB-A6B5-432F-8A47-D0566310210A', '5F6B076C-526F-4F65-B5E3-F416520D1841', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 5, 7, '2026-05-02 20:15:00', '2026-05-02 12:00:00'),
('865CF343-0140-45ED-8661-A42ACFAE0C78', 'C07977DB-A6B5-432F-8A47-D0566310210A', '5F6B076C-526F-4F65-B5E3-F416520D1841', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 6, 9, '2026-05-15 06:03:00', '2026-05-14 12:00:00'),
('C36FFFC8-60E4-41E7-A835-7A77CE7278B8', 'C07977DB-A6B5-432F-8A47-D0566310210A', '5F6B076C-526F-4F65-B5E3-F416520D1841', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 7, 10, '2026-05-27 06:48:00', '2026-05-26 12:00:00'),
('79D0E0DD-C299-4FCC-B2D5-C9CD0BB1247A', '815C74CD-0F24-4C5E-A8D2-327F78507FE2', '5F6B076C-526F-4F65-B5E3-F416520D1841', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 3, 5, '2026-03-16 00:00:00', '2026-03-15 12:00:00'),
('C27BCD65-B2F4-4A67-8BBA-408D29FD0E75', '815C74CD-0F24-4C5E-A8D2-327F78507FE2', '5F6B076C-526F-4F65-B5E3-F416520D1841', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 4, 7, '2026-03-23 00:18:00', '2026-03-22 12:00:00'),
('AA113D45-8E1F-41B3-B53A-029068FB8111', '815C74CD-0F24-4C5E-A8D2-327F78507FE2', '5F6B076C-526F-4F65-B5E3-F416520D1841', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 3, 5, '2026-03-28 20:36:00', '2026-03-28 12:00:00'),
('BB698589-EEB8-47E7-9C18-D5FE7F770E1C', '815C74CD-0F24-4C5E-A8D2-327F78507FE2', '5F6B076C-526F-4F65-B5E3-F416520D1841', N'Partial', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 3, 3, '2026-04-05 07:20:00', '2026-04-04 12:00:00'),
('6A078C14-BB2F-4534-BF48-A4BE02EAE607', '815C74CD-0F24-4C5E-A8D2-327F78507FE2', '5F6B076C-526F-4F65-B5E3-F416520D1841', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 6, 10, '2026-04-12 09:55:00', '2026-04-11 12:00:00'),
('19B4AFB0-FDFB-4511-B01B-4B7F0F9E1D65', '815C74CD-0F24-4C5E-A8D2-327F78507FE2', '5F6B076C-526F-4F65-B5E3-F416520D1841', N'Partial', N'شعرت بهدوء أكبر بعد التمرين.', 4, 5, '2026-04-19 06:48:00', '2026-04-18 12:00:00'),
('185EF429-3D1F-4030-BFF2-39F6A8261B41', '815C74CD-0F24-4C5E-A8D2-327F78507FE2', '5F6B076C-526F-4F65-B5E3-F416520D1841', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 6, 8, '2026-04-24 22:22:00', '2026-04-24 12:00:00'),
('18492CC3-DC12-46D6-AF42-791B7773A732', '815C74CD-0F24-4C5E-A8D2-327F78507FE2', '5F6B076C-526F-4F65-B5E3-F416520D1841', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 5, 9, '2026-05-02 00:37:00', '2026-05-01 12:00:00'),
('5CEB3EE6-317B-424B-8BB5-07B3F720FF07', '033CF2A8-DD98-45DF-81F0-A078E05427D3', '5F6B076C-526F-4F65-B5E3-F416520D1841', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 3, 7, '2026-03-16 02:52:00', '2026-03-15 12:00:00'),
('4B0E55EF-3646-4244-BC33-AE484A486CBA', '033CF2A8-DD98-45DF-81F0-A078E05427D3', '5F6B076C-526F-4F65-B5E3-F416520D1841', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 2, 4, '2026-03-23 22:24:00', '2026-03-23 12:00:00'),
('9B10235A-76E0-4097-AD8C-821437311ADB', '033CF2A8-DD98-45DF-81F0-A078E05427D3', '5F6B076C-526F-4F65-B5E3-F416520D1841', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 5, 7, '2026-04-02 09:14:00', '2026-04-01 12:00:00'),
('FE731E84-7C7D-488D-BE66-D886825807D9', '033CF2A8-DD98-45DF-81F0-A078E05427D3', '5F6B076C-526F-4F65-B5E3-F416520D1841', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 5, 8, '2026-04-10 06:29:00', '2026-04-09 12:00:00'),
('FC71EEFF-7EAB-440F-A2BA-B557549642FE', '033CF2A8-DD98-45DF-81F0-A078E05427D3', '5F6B076C-526F-4F65-B5E3-F416520D1841', N'Partial', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 5, 5, '2026-04-18 08:27:00', '2026-04-17 12:00:00'),
('435FAF30-80FE-442B-BE30-727D7E43F190', '033CF2A8-DD98-45DF-81F0-A078E05427D3', '5F6B076C-526F-4F65-B5E3-F416520D1841', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 6, 10, '2026-04-26 00:28:00', '2026-04-25 12:00:00'),
('96504B8B-05E7-4920-8698-8DBD31A04A2E', '033CF2A8-DD98-45DF-81F0-A078E05427D3', '5F6B076C-526F-4F65-B5E3-F416520D1841', N'Partial', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 6, 8, '2026-05-05 01:25:00', '2026-05-04 12:00:00'),
('7BA5183C-D8F3-41D0-963F-4CFE90C9942D', '033CF2A8-DD98-45DF-81F0-A078E05427D3', '5F6B076C-526F-4F65-B5E3-F416520D1841', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 7, 10, '2026-05-12 23:16:00', '2026-05-12 12:00:00'),
('36C22ABF-B185-4E75-8679-D6A816C73473', 'E8547BAA-F924-4FEA-9974-0A72A3FF1BB9', '5F6B076C-526F-4F65-B5E3-F416520D1841', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 3, 7, '2026-03-18 06:13:00', '2026-03-17 12:00:00'),
('6C199F14-9BEA-404D-AFAA-435C6A95C1A0', 'E8547BAA-F924-4FEA-9974-0A72A3FF1BB9', '5F6B076C-526F-4F65-B5E3-F416520D1841', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 5, 9, '2026-03-27 21:38:00', '2026-03-27 12:00:00'),
('3DC6BC42-B4B7-434F-AA96-7AF4CAF863F3', 'E8547BAA-F924-4FEA-9974-0A72A3FF1BB9', '5F6B076C-526F-4F65-B5E3-F416520D1841', N'Partial', N'شعرت بهدوء أكبر بعد التمرين.', 4, 4, '2026-04-08 01:51:00', '2026-04-07 12:00:00'),
('7E479BF0-4956-42BA-84B7-A0018E87C5C1', 'E8547BAA-F924-4FEA-9974-0A72A3FF1BB9', '5F6B076C-526F-4F65-B5E3-F416520D1841', N'Skipped', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 4, 3, '2026-04-18 03:41:00', '2026-04-17 12:00:00'),
('DF67618E-5341-4FB0-971B-802AACA246F2', 'E8547BAA-F924-4FEA-9974-0A72A3FF1BB9', '5F6B076C-526F-4F65-B5E3-F416520D1841', N'Partial', N'شعرت بهدوء أكبر بعد التمرين.', 6, 6, '2026-04-28 01:52:00', '2026-04-27 12:00:00'),
('A421BB5D-7371-45D4-A79C-C6C6F592F87F', '718342AD-DE69-42A4-8FD5-EC6FA6C1AAA5', '8BDD5425-225E-4657-AF47-CA87A9625781', N'Skipped', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 3, 4, '2025-11-26 09:35:00', '2025-11-25 12:00:00'),
('6B80074F-C98D-41B4-BA32-091FEAF22247', '718342AD-DE69-42A4-8FD5-EC6FA6C1AAA5', '8BDD5425-225E-4657-AF47-CA87A9625781', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 5, 7, '2025-12-02 04:02:00', '2025-12-01 12:00:00'),
('52B0D579-A145-4298-ADDB-92AD1157EB47', '718342AD-DE69-42A4-8FD5-EC6FA6C1AAA5', '8BDD5425-225E-4657-AF47-CA87A9625781', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 3, 7, '2025-12-09 01:14:00', '2025-12-08 12:00:00'),
('BDB484E1-DE76-478E-9E9A-4C323AB6EBF1', '718342AD-DE69-42A4-8FD5-EC6FA6C1AAA5', '8BDD5425-225E-4657-AF47-CA87A9625781', N'Partial', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 5, 6, '2025-12-15 07:57:00', '2025-12-14 12:00:00'),
('0509E057-A772-497A-B656-D0CC66EE7884', '718342AD-DE69-42A4-8FD5-EC6FA6C1AAA5', '8BDD5425-225E-4657-AF47-CA87A9625781', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 4, 7, '2025-12-22 06:07:00', '2025-12-21 12:00:00'),
('8BBBC6B5-E3D0-46DA-A43B-CCFC99AC3E9E', '718342AD-DE69-42A4-8FD5-EC6FA6C1AAA5', '8BDD5425-225E-4657-AF47-CA87A9625781', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 5, 9, '2025-12-28 04:17:00', '2025-12-27 12:00:00'),
('4E6A8FDA-EA01-44CE-87BC-6679CF5A7EB1', 'EA90646C-E097-4CF8-82AA-CFD27F8D743C', '8BDD5425-225E-4657-AF47-CA87A9625781', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 4, 6, '2025-11-28 23:19:00', '2025-11-28 12:00:00'),
('06053B11-0C1B-4A86-BC1B-71836A8B458C', 'EA90646C-E097-4CF8-82AA-CFD27F8D743C', '8BDD5425-225E-4657-AF47-CA87A9625781', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 2, 4, '2025-12-10 06:11:00', '2025-12-09 12:00:00'),
('A1EFA4E8-13EF-4DE7-9E4F-7523458E5623', 'EA90646C-E097-4CF8-82AA-CFD27F8D743C', '8BDD5425-225E-4657-AF47-CA87A9625781', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 5, 9, '2025-12-20 07:50:00', '2025-12-19 12:00:00'),
('8EB78562-EBA0-4026-9038-BD2339EEE294', 'EA90646C-E097-4CF8-82AA-CFD27F8D743C', '8BDD5425-225E-4657-AF47-CA87A9625781', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 5, 7, '2025-12-30 23:17:00', '2025-12-30 12:00:00'),
('B3CD69C4-F5A3-48CA-B2F2-BC874E2F26E2', 'EA90646C-E097-4CF8-82AA-CFD27F8D743C', '8BDD5425-225E-4657-AF47-CA87A9625781', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 6, 8, '2026-01-11 04:23:00', '2026-01-10 12:00:00'),
('950DEC7F-B21A-4D69-BE42-7DA037DBEEC9', 'EA90646C-E097-4CF8-82AA-CFD27F8D743C', '8BDD5425-225E-4657-AF47-CA87A9625781', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 5, 7, '2026-01-22 06:29:00', '2026-01-21 12:00:00'),
('9B63C8F5-CDFE-46C6-8BDE-FD294D01FF08', 'EA90646C-E097-4CF8-82AA-CFD27F8D743C', '8BDD5425-225E-4657-AF47-CA87A9625781', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 7, 9, '2026-02-01 08:31:00', '2026-01-31 12:00:00'),
('E1481D78-29C9-4E28-B3A3-97CEBF2E332F', 'EA90646C-E097-4CF8-82AA-CFD27F8D743C', '8BDD5425-225E-4657-AF47-CA87A9625781', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 7, 10, '2026-02-12 07:49:00', '2026-02-11 12:00:00'),
('CB9F8163-7270-4FD4-BB13-D119E6FA5A20', 'B17D7BEF-A5B7-4C08-851D-94DE56C45652', '8BDD5425-225E-4657-AF47-CA87A9625781', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 2, 5, '2025-11-25 21:02:00', '2025-11-25 12:00:00'),
('FF37560E-BE00-41EB-8140-F0AD3601BEF7', 'B17D7BEF-A5B7-4C08-851D-94DE56C45652', '8BDD5425-225E-4657-AF47-CA87A9625781', N'Partial', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 4, 5, '2025-12-07 21:08:00', '2025-12-07 12:00:00'),
('AEFC9BC1-AA41-484E-857D-765DCF749A46', 'B17D7BEF-A5B7-4C08-851D-94DE56C45652', '8BDD5425-225E-4657-AF47-CA87A9625781', N'Partial', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 5, 6, '2025-12-19 04:22:00', '2025-12-18 12:00:00'),
('AE1B5102-9119-492E-BCE4-B2F39EEAE774', 'B17D7BEF-A5B7-4C08-851D-94DE56C45652', '8BDD5425-225E-4657-AF47-CA87A9625781', N'Partial', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 4, 6, '2025-12-31 07:51:00', '2025-12-30 12:00:00'),
('E27483C8-0F60-455C-8138-F95D68511928', 'B17D7BEF-A5B7-4C08-851D-94DE56C45652', '8BDD5425-225E-4657-AF47-CA87A9625781', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 4, 8, '2026-01-11 02:58:00', '2026-01-10 12:00:00'),
('B2F1F49B-086C-4108-8FFA-141541E5E0CE', 'B17D7BEF-A5B7-4C08-851D-94DE56C45652', '8BDD5425-225E-4657-AF47-CA87A9625781', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 4, 7, '2026-01-23 00:54:00', '2026-01-22 12:00:00'),
('FB8D9220-288D-4A1C-847F-1F4231EBDCE3', 'B17D7BEF-A5B7-4C08-851D-94DE56C45652', '8BDD5425-225E-4657-AF47-CA87A9625781', N'Partial', N'شعرت بهدوء أكبر بعد التمرين.', 7, 8, '2026-02-03 02:23:00', '2026-02-02 12:00:00'),
('FF89018B-16A2-42FA-B8C4-A82F8296919E', 'B17D7BEF-A5B7-4C08-851D-94DE56C45652', '8BDD5425-225E-4657-AF47-CA87A9625781', N'Completed', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 6, 8, '2026-02-15 03:10:00', '2026-02-14 12:00:00'),
('A2D6A5CF-A0E2-491E-BAA4-99F7E9887A13', '677D9929-6814-4C1A-A5AF-95198D73A66A', 'D149A229-6389-48F8-91B0-BE1D2E2900B1', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 3, 6, '2025-09-13 02:24:00', '2025-09-12 12:00:00'),
('1EC6CBAD-2355-418C-A36F-E203F157DB99', '677D9929-6814-4C1A-A5AF-95198D73A66A', 'D149A229-6389-48F8-91B0-BE1D2E2900B1', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 3, 5, '2025-09-18 22:58:00', '2025-09-18 12:00:00'),
('A3AF7760-16D5-43BD-9603-0557340D999E', '677D9929-6814-4C1A-A5AF-95198D73A66A', 'D149A229-6389-48F8-91B0-BE1D2E2900B1', N'Partial', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 3, 3, '2025-09-25 07:26:00', '2025-09-24 12:00:00'),
('595172AD-AB81-45F8-88E3-8F8018653A9A', '677D9929-6814-4C1A-A5AF-95198D73A66A', 'D149A229-6389-48F8-91B0-BE1D2E2900B1', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 6, 8, '2025-10-01 05:30:00', '2025-09-30 12:00:00'),
('B47D30C7-8CEB-45C9-8A75-93AA4ED1222E', '677D9929-6814-4C1A-A5AF-95198D73A66A', 'D149A229-6389-48F8-91B0-BE1D2E2900B1', N'Partial', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 5, 7, '2025-10-07 06:33:00', '2025-10-06 12:00:00'),
('16699016-02C0-4191-B2C9-7B57C2864259', '677D9929-6814-4C1A-A5AF-95198D73A66A', 'D149A229-6389-48F8-91B0-BE1D2E2900B1', N'Skipped', N'أصبح التمرين أسهل وأشعر بفائدته بشكل متزايد.', 5, 4, '2025-10-12 20:15:00', '2025-10-12 12:00:00'),
('96D31CD8-BD9F-46EA-98D1-5063CAEAB8A5', '1BAD9CCD-512F-4199-8843-941D75781975', 'D149A229-6389-48F8-91B0-BE1D2E2900B1', N'Completed', N'أحتاج وقتاً أطول لإتقان هذا التمرين.', 3, 7, '2025-09-07 08:07:00', '2025-09-06 12:00:00'),
('BC0639C7-ECD7-495D-AD8F-085C8154C52B', '1BAD9CCD-512F-4199-8843-941D75781975', 'D149A229-6389-48F8-91B0-BE1D2E2900B1', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 3, 7, '2025-09-16 04:47:00', '2025-09-15 12:00:00'),
('C211484A-7601-43D5-A6B3-863552F6973F', '1BAD9CCD-512F-4199-8843-941D75781975', 'D149A229-6389-48F8-91B0-BE1D2E2900B1', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 4, 8, '2025-09-24 21:24:00', '2025-09-24 12:00:00'),
('7289EE16-0B97-4454-B526-9A08A43E817A', '1BAD9CCD-512F-4199-8843-941D75781975', 'D149A229-6389-48F8-91B0-BE1D2E2900B1', N'Partial', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 6, 8, '2025-10-04 06:49:00', '2025-10-03 12:00:00'),
('B7EEFC13-B386-4A7E-9CD1-5387360339F3', '1BAD9CCD-512F-4199-8843-941D75781975', 'D149A229-6389-48F8-91B0-BE1D2E2900B1', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 6, 9, '2025-10-13 06:13:00', '2025-10-12 12:00:00'),
('A010047F-86AA-4468-BF9E-E66FD19C8D0C', '1BAD9CCD-512F-4199-8843-941D75781975', 'D149A229-6389-48F8-91B0-BE1D2E2900B1', N'Completed', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 5, 8, '2025-10-21 20:32:00', '2025-10-21 12:00:00'),
('AEC9688E-FB59-4734-968C-1A441B195482', 'C02A2BFC-6B2C-418F-8C62-73F17342AD0B', 'D149A229-6389-48F8-91B0-BE1D2E2900B1', N'Partial', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 2, 2, '2025-09-07 23:00:00', '2025-09-07 12:00:00'),
('8D869176-F1BB-403A-A543-C25600EAC6AD', 'C02A2BFC-6B2C-418F-8C62-73F17342AD0B', 'D149A229-6389-48F8-91B0-BE1D2E2900B1', N'Completed', N'شعرت بتوتر أثناء التمرين ولم ألاحظ فرقاً كبيراً بعد.', 4, 7, '2025-09-19 21:50:00', '2025-09-19 12:00:00'),
('4522DD8B-31DA-4F83-B96E-0760250BCE11', 'C02A2BFC-6B2C-418F-8C62-73F17342AD0B', 'D149A229-6389-48F8-91B0-BE1D2E2900B1', N'Completed', N'كان من الصعب التركيز اليوم لكن حاولت الاستمرار.', 4, 8, '2025-10-01 09:05:00', '2025-09-30 12:00:00'),
('6A823369-EBE0-43B8-94A3-4885F42DAEC8', 'C02A2BFC-6B2C-418F-8C62-73F17342AD0B', 'D149A229-6389-48F8-91B0-BE1D2E2900B1', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 3, 7, '2025-10-13 04:58:00', '2025-10-12 12:00:00'),
('F79341FE-AA81-41EF-BB96-97F72782AC03', 'C02A2BFC-6B2C-418F-8C62-73F17342AD0B', 'D149A229-6389-48F8-91B0-BE1D2E2900B1', N'Completed', N'لاحظت انخفاضاً واضحاً في التوتر الجسدي.', 6, 9, '2025-10-24 08:51:00', '2025-10-23 12:00:00'),
('4CD32FE6-1FF7-47DA-80AE-25B18D0019E3', 'C02A2BFC-6B2C-418F-8C62-73F17342AD0B', 'D149A229-6389-48F8-91B0-BE1D2E2900B1', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 6, 9, '2025-11-04 21:20:00', '2025-11-04 12:00:00'),
('8E387529-CA4D-40C9-9868-753B1B38147C', 'C02A2BFC-6B2C-418F-8C62-73F17342AD0B', 'D149A229-6389-48F8-91B0-BE1D2E2900B1', N'Completed', N'شعرت بهدوء أكبر بعد التمرين.', 7, 10, '2025-11-16 08:12:00', '2025-11-15 12:00:00'),
('ED5103B6-B951-479B-9075-75F89E6B18EC', 'C02A2BFC-6B2C-418F-8C62-73F17342AD0B', 'D149A229-6389-48F8-91B0-BE1D2E2900B1', N'Partial', N'شعرت بتحسن ملحوظ في المزاج بعد التمرين.', 6, 7, '2025-11-28 00:07:00', '2025-11-27 12:00:00');

-- ===== ReferralReports =====
INSERT INTO [ReferralReports] ([Id], [PatientId], [TherapistId], [GeneratedByTherapistId], [Status], [CurrentVersionId], [CreatedAt], [UpdatedAt])
VALUES
('9037759B-DD09-4E05-93DE-EEFDC968183A', '692BFE83-C7BF-4FAA-9245-0EEDE315EBDB', '6251ABB6-B294-48CA-9B75-4E959CFA04A7', '6251ABB6-B294-48CA-9B75-4E959CFA04A7', N'InReview', NULL, '2026-07-24 12:00:00', '2026-07-24 12:00:00'),
('50A7CB45-0345-437C-AC52-5049A91B5C8B', '5912AA70-D127-4B2A-95B6-62222A464244', '0D30427F-E161-441E-AA05-F6749620088C', '0D30427F-E161-441E-AA05-F6749620088C', N'Sent', NULL, '2026-06-30 12:00:00', '2026-06-30 12:00:00'),
('7FB31C31-E0F5-40FC-9BB9-7EA1C9B2949A', '5912AA70-D127-4B2A-95B6-62222A464244', '0D30427F-E161-441E-AA05-F6749620088C', '0D30427F-E161-441E-AA05-F6749620088C', N'Draft', NULL, '2026-06-26 12:00:00', '2026-06-26 12:00:00'),
('5D59D23C-1A81-48EC-85B1-F412BAAC78FC', 'D091A7F4-A73A-4CCE-920E-0AB7D4C36536', '25A9DCC1-6815-47F4-86AD-D25FF47B7ADE', '25A9DCC1-6815-47F4-86AD-D25FF47B7ADE', N'InReview', NULL, '2026-06-19 12:00:00', '2026-06-19 12:00:00'),
('88836645-F9A6-4988-9CFC-9107C85761E5', '3A5350AB-011B-41CA-A9F0-A7B2289AE70C', 'DDDA597C-2D47-4C64-914D-6DF694AA3606', 'DDDA597C-2D47-4C64-914D-6DF694AA3606', N'Sent', NULL, '2026-01-22 12:00:00', '2026-01-22 12:00:00'),
('FED06835-0677-457B-BA44-8FA599F96551', '3A5350AB-011B-41CA-A9F0-A7B2289AE70C', 'DDDA597C-2D47-4C64-914D-6DF694AA3606', 'DDDA597C-2D47-4C64-914D-6DF694AA3606', N'InReview', NULL, '2026-06-27 12:00:00', '2026-06-27 12:00:00'),
('0521C9DF-D099-4D9B-9AA6-0B5D8D3295BA', 'E07477FB-C7F6-4846-A2AB-0ED1F6DCB7AC', '899B50C5-FFCF-4E6E-964D-3A59908E422E', '899B50C5-FFCF-4E6E-964D-3A59908E422E', N'Draft', NULL, '2025-11-06 12:00:00', '2025-11-06 12:00:00'),
('011FB171-19B5-4F5C-9827-245749DAAD9E', 'DC19C765-09E1-4B24-808B-8EFC97E63589', 'E73DBDDC-8CD8-42F3-A385-E5D24710E59F', 'E73DBDDC-8CD8-42F3-A385-E5D24710E59F', N'InReview', NULL, '2026-06-26 12:00:00', '2026-06-26 12:00:00'),
('340B9B99-805C-4E87-A154-7F5EB394C69B', '8F8DC8EB-DCBE-44D2-B913-73C2CE8E75B3', '770DD106-350E-40A8-B1BC-4CD0BED58C40', '770DD106-350E-40A8-B1BC-4CD0BED58C40', N'InReview', NULL, '2026-05-10 12:00:00', '2026-05-10 12:00:00'),
('C866085E-955E-469F-8F0E-AB148E31852F', 'F9DD6265-AFC8-44EC-8985-D0E90571FCA6', '9F442928-B53E-4FDC-B241-988969B52203', '9F442928-B53E-4FDC-B241-988969B52203', N'Draft', NULL, '2026-03-31 12:00:00', '2026-03-31 12:00:00'),
('D8782544-C6AA-4374-B054-1E34A040F481', 'A8BC8AD9-EC06-4493-8502-F88E2C965073', '6251ABB6-B294-48CA-9B75-4E959CFA04A7', '6251ABB6-B294-48CA-9B75-4E959CFA04A7', N'Finalized', NULL, '2026-07-06 12:00:00', '2026-07-06 12:00:00'),
('162BDB21-6FE6-4FB2-ABBC-C415D57B7542', 'A8BC8AD9-EC06-4493-8502-F88E2C965073', '6251ABB6-B294-48CA-9B75-4E959CFA04A7', '6251ABB6-B294-48CA-9B75-4E959CFA04A7', N'Draft', NULL, '2026-07-07 12:00:00', '2026-07-07 12:00:00'),
('B0E7BA52-BC5B-4ADC-BE7C-EFE644724E1C', '2557FC9B-7599-4EB3-9FA4-7DCD1EAE6EDA', '0D30427F-E161-441E-AA05-F6749620088C', '0D30427F-E161-441E-AA05-F6749620088C', N'Finalized', NULL, '2026-06-16 12:00:00', '2026-06-16 12:00:00'),
('D4A7EA75-2269-4463-B524-116D12542BA7', '2557FC9B-7599-4EB3-9FA4-7DCD1EAE6EDA', '0D30427F-E161-441E-AA05-F6749620088C', '0D30427F-E161-441E-AA05-F6749620088C', N'Draft', NULL, '2026-03-16 12:00:00', '2026-03-16 12:00:00'),
('91B87AB5-BC37-4D8B-A4FA-EFF6173EA91F', '87C46097-7D04-4AD1-89C2-13967E95301F', '25A9DCC1-6815-47F4-86AD-D25FF47B7ADE', '25A9DCC1-6815-47F4-86AD-D25FF47B7ADE', N'Finalized', NULL, '2026-06-29 12:00:00', '2026-06-29 12:00:00'),
('510C8B71-6877-4E6F-84AA-E917F481F113', '87C46097-7D04-4AD1-89C2-13967E95301F', '25A9DCC1-6815-47F4-86AD-D25FF47B7ADE', '25A9DCC1-6815-47F4-86AD-D25FF47B7ADE', N'Draft', NULL, '2026-05-28 12:00:00', '2026-05-28 12:00:00'),
('3456AD03-6B6C-48A3-AF98-53D98C25411C', '0B875E9D-0EE5-40EF-9CC6-451259EEADB3', 'DDDA597C-2D47-4C64-914D-6DF694AA3606', 'DDDA597C-2D47-4C64-914D-6DF694AA3606', N'Sent', NULL, '2026-04-06 12:00:00', '2026-04-06 12:00:00'),
('242950B2-405D-4FD9-AE08-EC4E0C5F3C4E', '0B875E9D-0EE5-40EF-9CC6-451259EEADB3', 'DDDA597C-2D47-4C64-914D-6DF694AA3606', 'DDDA597C-2D47-4C64-914D-6DF694AA3606', N'Draft', NULL, '2026-03-29 12:00:00', '2026-03-29 12:00:00'),
('53233CCE-8047-470F-AB01-DA92E0046417', 'ED4A6B23-9515-4C85-9110-1C4CC21FAA9E', '899B50C5-FFCF-4E6E-964D-3A59908E422E', '899B50C5-FFCF-4E6E-964D-3A59908E422E', N'Sent', NULL, '2026-02-25 12:00:00', '2026-02-25 12:00:00'),
('E5762594-8C56-41DD-A0F5-A74327F26AEB', 'ED4A6B23-9515-4C85-9110-1C4CC21FAA9E', '899B50C5-FFCF-4E6E-964D-3A59908E422E', '899B50C5-FFCF-4E6E-964D-3A59908E422E', N'InReview', NULL, '2026-06-16 12:00:00', '2026-06-16 12:00:00'),
('485A62C3-9237-426A-87A7-173E6F99C1EE', '69F104EE-9B40-40CF-A029-EC2A532693FA', 'E73DBDDC-8CD8-42F3-A385-E5D24710E59F', 'E73DBDDC-8CD8-42F3-A385-E5D24710E59F', N'InReview', NULL, '2026-04-28 12:00:00', '2026-04-28 12:00:00'),
('01BA512C-A517-41E2-BB48-FF2295EFA18C', '56A8CAE5-A2FA-4149-B2D0-3E5B35F37D3B', '770DD106-350E-40A8-B1BC-4CD0BED58C40', '770DD106-350E-40A8-B1BC-4CD0BED58C40', N'InReview', NULL, '2026-06-24 12:00:00', '2026-06-24 12:00:00'),
('2536E371-05F2-41B5-8A64-0492D91F5A2E', '85B46DDD-B1A6-4148-BB18-D7DB4172EAF2', '9F442928-B53E-4FDC-B241-988969B52203', '9F442928-B53E-4FDC-B241-988969B52203', N'Sent', NULL, '2026-06-18 12:00:00', '2026-06-18 12:00:00'),
('B6D5F971-A601-41E7-9B07-411316C0763D', '85B46DDD-B1A6-4148-BB18-D7DB4172EAF2', '9F442928-B53E-4FDC-B241-988969B52203', '9F442928-B53E-4FDC-B241-988969B52203', N'Draft', NULL, '2025-11-24 12:00:00', '2025-11-24 12:00:00'),
('1631C229-FCD6-4A13-8684-3397A758A287', 'D5AF7A7D-26C9-4CD6-854B-11C4F6A65218', '6251ABB6-B294-48CA-9B75-4E959CFA04A7', '6251ABB6-B294-48CA-9B75-4E959CFA04A7', N'Sent', NULL, '2026-06-09 12:00:00', '2026-06-09 12:00:00'),
('81B8ED6A-0DD5-473F-A952-C60258EA73CE', 'D5AF7A7D-26C9-4CD6-854B-11C4F6A65218', '6251ABB6-B294-48CA-9B75-4E959CFA04A7', '6251ABB6-B294-48CA-9B75-4E959CFA04A7', N'InReview', NULL, '2025-11-28 12:00:00', '2025-11-28 12:00:00'),
('143D58A2-DB82-4082-8349-DD7D2B9B48CC', 'AEB38627-5539-4776-8AEC-69F91DD159F1', '0D30427F-E161-441E-AA05-F6749620088C', '0D30427F-E161-441E-AA05-F6749620088C', N'InReview', NULL, '2026-05-25 12:00:00', '2026-05-25 12:00:00'),
('27E7CB86-589F-46D4-B1AC-B151CE267968', '00CC925F-9671-4970-ADC6-5C26F8FBF8E5', '25A9DCC1-6815-47F4-86AD-D25FF47B7ADE', '25A9DCC1-6815-47F4-86AD-D25FF47B7ADE', N'Draft', NULL, '2026-07-03 12:00:00', '2026-07-03 12:00:00'),
('A645A80E-1DCC-47E4-BCF8-5C1B7E0ADAF7', '5947B1E9-EACB-4CD1-A014-69DBDA364362', 'DDDA597C-2D47-4C64-914D-6DF694AA3606', 'DDDA597C-2D47-4C64-914D-6DF694AA3606', N'Finalized', NULL, '2026-02-17 12:00:00', '2026-02-17 12:00:00'),
('B4E0D6DA-2D97-4C63-9EA4-5B2025144B2F', '5947B1E9-EACB-4CD1-A014-69DBDA364362', 'DDDA597C-2D47-4C64-914D-6DF694AA3606', 'DDDA597C-2D47-4C64-914D-6DF694AA3606', N'InReview', NULL, '2026-02-09 12:00:00', '2026-02-09 12:00:00'),
('66E238E6-AB5C-4379-A200-8D79516CDBF7', 'D05B32B7-0D94-4041-91CE-8952FF6C525A', '899B50C5-FFCF-4E6E-964D-3A59908E422E', '899B50C5-FFCF-4E6E-964D-3A59908E422E', N'Sent', NULL, '2025-12-07 12:00:00', '2025-12-07 12:00:00'),
('F929250B-89D0-46EA-9781-4A199C59F8F6', 'D05B32B7-0D94-4041-91CE-8952FF6C525A', '899B50C5-FFCF-4E6E-964D-3A59908E422E', '899B50C5-FFCF-4E6E-964D-3A59908E422E', N'Draft', NULL, '2026-03-16 12:00:00', '2026-03-16 12:00:00'),
('9C2CAE46-BEAA-4621-9474-8EFFA8D16289', '7F9C808B-66FB-44EC-AE2F-D987CCB0D98A', 'E73DBDDC-8CD8-42F3-A385-E5D24710E59F', 'E73DBDDC-8CD8-42F3-A385-E5D24710E59F', N'InReview', NULL, '2026-04-11 12:00:00', '2026-04-11 12:00:00'),
('971B3A36-5EBA-4921-939F-DA23E52698F4', '080C5383-4556-405D-BBEC-38C843811EBD', '770DD106-350E-40A8-B1BC-4CD0BED58C40', '770DD106-350E-40A8-B1BC-4CD0BED58C40', N'Finalized', NULL, '2026-07-18 12:00:00', '2026-07-18 12:00:00'),
('36CDF84D-A8D6-4D7C-AB08-CB96F6CFB3F3', '080C5383-4556-405D-BBEC-38C843811EBD', '770DD106-350E-40A8-B1BC-4CD0BED58C40', '770DD106-350E-40A8-B1BC-4CD0BED58C40', N'InReview', NULL, '2026-07-18 12:00:00', '2026-07-18 12:00:00'),
('0F2C1F23-7820-4C90-9FBE-6A53C21F70B3', '5A0DA59E-4AE0-4CF6-B7CD-5BDB2F91D5E7', '9F442928-B53E-4FDC-B241-988969B52203', '9F442928-B53E-4FDC-B241-988969B52203', N'Finalized', NULL, '2026-05-08 12:00:00', '2026-05-08 12:00:00'),
('1A437683-F6C2-4148-985C-B48544E08DA4', '5A0DA59E-4AE0-4CF6-B7CD-5BDB2F91D5E7', '9F442928-B53E-4FDC-B241-988969B52203', '9F442928-B53E-4FDC-B241-988969B52203', N'Draft', NULL, '2026-02-26 12:00:00', '2026-02-26 12:00:00'),
('BE699D36-503B-4100-BA05-AE63DB4D2D2A', '0004E60D-C638-4639-BCFA-0AD58724C84E', '6251ABB6-B294-48CA-9B75-4E959CFA04A7', '6251ABB6-B294-48CA-9B75-4E959CFA04A7', N'Sent', NULL, '2026-07-11 12:00:00', '2026-07-11 12:00:00'),
('BF1CE234-9C8A-4A2C-A5B7-5A56573F9E16', '0004E60D-C638-4639-BCFA-0AD58724C84E', '6251ABB6-B294-48CA-9B75-4E959CFA04A7', '6251ABB6-B294-48CA-9B75-4E959CFA04A7', N'InReview', NULL, '2026-07-11 12:00:00', '2026-07-11 12:00:00'),
('2672DDC7-E1D5-4625-9712-1D54F1DC0AD8', '81B640AC-DBBD-4F29-831E-F36B5C6A7580', '0D30427F-E161-441E-AA05-F6749620088C', '0D30427F-E161-441E-AA05-F6749620088C', N'Sent', NULL, '2026-04-01 12:00:00', '2026-04-01 12:00:00'),
('40A0AD1E-96FC-4314-8A24-91A989A2EAA5', '81B640AC-DBBD-4F29-831E-F36B5C6A7580', '0D30427F-E161-441E-AA05-F6749620088C', '0D30427F-E161-441E-AA05-F6749620088C', N'Draft', NULL, '2026-05-31 12:00:00', '2026-05-31 12:00:00'),
('F523819E-1302-441C-95C4-4DA2138F560A', '85C8A7BC-BBE9-4D03-B8D2-89AFEE304102', '25A9DCC1-6815-47F4-86AD-D25FF47B7ADE', '25A9DCC1-6815-47F4-86AD-D25FF47B7ADE', N'InReview', NULL, '2026-03-04 12:00:00', '2026-03-04 12:00:00'),
('454EAE95-4D2D-48AA-9BD4-9FD089BF1E3E', '46ACC094-18A3-4CCF-80C4-95D298EF4031', 'DDDA597C-2D47-4C64-914D-6DF694AA3606', 'DDDA597C-2D47-4C64-914D-6DF694AA3606', N'Finalized', NULL, '2026-07-02 12:00:00', '2026-07-02 12:00:00'),
('95C4470D-DEB7-4903-9839-397BF73D8326', '46ACC094-18A3-4CCF-80C4-95D298EF4031', 'DDDA597C-2D47-4C64-914D-6DF694AA3606', 'DDDA597C-2D47-4C64-914D-6DF694AA3606', N'InReview', NULL, '2026-05-23 12:00:00', '2026-05-23 12:00:00'),
('4B6CC5D7-410D-4AD1-9EC5-8577AB00F703', 'F300A791-B73E-4A86-90EA-4D1EE67BBE48', '899B50C5-FFCF-4E6E-964D-3A59908E422E', '899B50C5-FFCF-4E6E-964D-3A59908E422E', N'Finalized', NULL, '2026-06-24 12:00:00', '2026-06-24 12:00:00'),
('5F99E030-A134-4727-88A8-345EB021F6DB', 'F300A791-B73E-4A86-90EA-4D1EE67BBE48', '899B50C5-FFCF-4E6E-964D-3A59908E422E', '899B50C5-FFCF-4E6E-964D-3A59908E422E', N'Draft', NULL, '2026-06-29 12:00:00', '2026-06-29 12:00:00'),
('92D82131-9AFA-4F0B-99FC-3ED47BB72F19', '74FDAC62-9C1A-472B-AA93-BCDC4103514D', 'E73DBDDC-8CD8-42F3-A385-E5D24710E59F', 'E73DBDDC-8CD8-42F3-A385-E5D24710E59F', N'Draft', NULL, '2026-04-16 12:00:00', '2026-04-16 12:00:00'),
('C5543C2E-4379-4B50-B811-869B705C85C7', '5A2EBBD8-21B9-47E7-881C-B09F16F70FE6', '770DD106-350E-40A8-B1BC-4CD0BED58C40', '770DD106-350E-40A8-B1BC-4CD0BED58C40', N'InReview', NULL, '2026-06-26 12:00:00', '2026-06-26 12:00:00'),
('3866A896-C50F-42FD-92AC-89371C5BB378', 'CC37E596-7078-4E46-938E-D6B59CA67B02', '9F442928-B53E-4FDC-B241-988969B52203', '9F442928-B53E-4FDC-B241-988969B52203', N'Finalized', NULL, '2026-07-01 12:00:00', '2026-07-01 12:00:00'),
('A125E586-DA08-40AA-AF94-CD4559A75D22', 'CC37E596-7078-4E46-938E-D6B59CA67B02', '9F442928-B53E-4FDC-B241-988969B52203', '9F442928-B53E-4FDC-B241-988969B52203', N'InReview', NULL, '2026-06-06 12:00:00', '2026-06-06 12:00:00'),
('6F314E07-F840-48AA-89BB-2C184AA9F6F4', '5F6B076C-526F-4F65-B5E3-F416520D1841', '6251ABB6-B294-48CA-9B75-4E959CFA04A7', '6251ABB6-B294-48CA-9B75-4E959CFA04A7', N'Draft', NULL, '2026-05-30 12:00:00', '2026-05-30 12:00:00'),
('70561C73-88AC-4F91-BE4C-52562AEC600D', '8BDD5425-225E-4657-AF47-CA87A9625781', '0D30427F-E161-441E-AA05-F6749620088C', '0D30427F-E161-441E-AA05-F6749620088C', N'Sent', NULL, '2026-05-07 12:00:00', '2026-05-07 12:00:00'),
('4B759911-A29D-48C3-A62F-188F48AC5823', '8BDD5425-225E-4657-AF47-CA87A9625781', '0D30427F-E161-441E-AA05-F6749620088C', '0D30427F-E161-441E-AA05-F6749620088C', N'InReview', NULL, '2026-06-15 12:00:00', '2026-06-15 12:00:00'),
('14F80424-06AD-404B-83FC-EE7DE475DACE', 'D149A229-6389-48F8-91B0-BE1D2E2900B1', '25A9DCC1-6815-47F4-86AD-D25FF47B7ADE', '25A9DCC1-6815-47F4-86AD-D25FF47B7ADE', N'Finalized', NULL, '2026-03-13 12:00:00', '2026-03-13 12:00:00'),
('9E7DECB1-2A41-4087-B4FF-7A8F7D802900', 'D149A229-6389-48F8-91B0-BE1D2E2900B1', '25A9DCC1-6815-47F4-86AD-D25FF47B7ADE', '25A9DCC1-6815-47F4-86AD-D25FF47B7ADE', N'Draft', NULL, '2025-10-02 12:00:00', '2025-10-02 12:00:00');

-- ===== ReportVersions =====
INSERT INTO [ReportVersions] ([Id], [ReportId], [VersionNumber], [Content], [CreatedByTherapistId], [ApprovedAt], [ChangeNote], [CreatedAt])
VALUES
('CC3CC2E9-EF69-4455-9731-43FC5802CAC9', '9037759B-DD09-4E05-93DE-EEFDC968183A', 1, N'تشير المؤشرات إلى انخفاض تدريجي في مستويات التوتر والضغط النفسي لدى المريض على مدار فترة المتابعة.

التوصيات:
- المتابعة الدورية كل أسبوعين
- ممارسة تمارين التنفس بانتظام

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', '6251ABB6-B294-48CA-9B75-4E959CFA04A7', NULL, N'النسخة الأولى', '2026-07-24 12:00:00'),
('B6D1592E-8CAB-444F-AB3E-D37230098823', '50A7CB45-0345-437C-AC52-5049A91B5C8B', 1, N'تشير المؤشرات إلى انخفاض تدريجي في مستويات التوتر والضغط النفسي لدى المريض على مدار فترة المتابعة.

التوصيات:
- المتابعة الدورية كل أسبوعين
- الاستمرار في تمارين العلاج السلوكي المعرفي (CBT)

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', '0D30427F-E161-441E-AA05-F6749620088C', '2026-07-01 12:00:00', N'النسخة الأولى', '2026-06-30 12:00:00'),
('0EA35E8D-31FB-4327-8DB1-F71CEED07E95', '7FB31C31-E0F5-40FC-9BB9-7EA1C9B2949A', 1, N'تشير المؤشرات إلى انخفاض تدريجي في مستويات التوتر والضغط النفسي لدى المريض على مدار فترة المتابعة.

التوصيات:
- ممارسة تمارين التنفس بانتظام
- تحسين روتين النوم

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', '0D30427F-E161-441E-AA05-F6749620088C', NULL, N'النسخة الأولى', '2026-06-26 12:00:00'),
('6D01861B-D498-4C05-A707-7AEFAC468341', '5D59D23C-1A81-48EC-85B1-F412BAAC78FC', 1, N'تُظهر الجلسات الأخيرة تحسناً تدريجياً في مستوى القلق العام لدى المريض، مع استجابة جيدة لتمارين الاسترخاء والتنفس.

التوصيات:
- المتابعة الدورية كل أسبوعين
- زيادة النشاط الاجتماعي التدريجي

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', '25A9DCC1-6815-47F4-86AD-D25FF47B7ADE', NULL, N'النسخة الأولى', '2026-06-19 12:00:00'),
('0136092F-B742-4C6D-B493-E676EF679BFC', '88836645-F9A6-4988-9CFC-9107C85761E5', 1, N'تُظهر الجلسات الأخيرة تحسناً تدريجياً في مستوى القلق العام لدى المريض، مع استجابة جيدة لتمارين الاسترخاء والتنفس.

التوصيات:
- ممارسة تمارين التنفس بانتظام
- زيادة النشاط الاجتماعي التدريجي

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', 'DDDA597C-2D47-4C64-914D-6DF694AA3606', '2026-01-23 12:00:00', N'النسخة الأولى', '2026-01-22 12:00:00'),
('49750132-AA60-4176-A8B5-6A0324C9B264', 'FED06835-0677-457B-BA44-8FA599F96551', 1, N'تُظهر الجلسات الأخيرة تحسناً تدريجياً في مستوى القلق العام لدى المريض، مع استجابة جيدة لتمارين الاسترخاء والتنفس.

التوصيات:
- الاستمرار في تمارين العلاج السلوكي المعرفي (CBT)
- المتابعة الدورية كل أسبوعين

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', 'DDDA597C-2D47-4C64-914D-6DF694AA3606', NULL, N'النسخة الأولى', '2026-06-27 12:00:00'),
('46D775BC-3F44-4319-BEDC-E33B98074E6A', '0521C9DF-D099-4D9B-9AA6-0B5D8D3295BA', 1, N'تُظهر الجلسات الأخيرة تحسناً تدريجياً في مستوى القلق العام لدى المريض، مع استجابة جيدة لتمارين الاسترخاء والتنفس.

التوصيات:
- ممارسة تمارين التنفس بانتظام
- زيادة النشاط الاجتماعي التدريجي

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', '899B50C5-FFCF-4E6E-964D-3A59908E422E', NULL, N'النسخة الأولى', '2025-11-06 12:00:00'),
('7DDFF79D-FD33-4827-B42E-2DD3AE2E8761', '011FB171-19B5-4F5C-9827-245749DAAD9E', 1, N'تشير المؤشرات إلى انخفاض تدريجي في مستويات التوتر والضغط النفسي لدى المريض على مدار فترة المتابعة.

التوصيات:
- تحسين روتين النوم
- ممارسة تمارين التنفس بانتظام

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', 'E73DBDDC-8CD8-42F3-A385-E5D24710E59F', NULL, N'النسخة الأولى', '2026-06-26 12:00:00'),
('42B8B0A1-C717-4F3C-A725-26AD46A00EEC', '340B9B99-805C-4E87-A154-7F5EB394C69B', 1, N'تُظهر الجلسات الأخيرة تحسناً تدريجياً في مستوى القلق العام لدى المريض، مع استجابة جيدة لتمارين الاسترخاء والتنفس.

التوصيات:
- المتابعة الدورية كل أسبوعين
- الاستمرار في تمارين العلاج السلوكي المعرفي (CBT)

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', '770DD106-350E-40A8-B1BC-4CD0BED58C40', NULL, N'النسخة الأولى', '2026-05-10 12:00:00'),
('E8B1CE59-BDD2-4C31-BA4A-57931FAFBD52', 'C866085E-955E-469F-8F0E-AB148E31852F', 1, N'تشير المؤشرات إلى انخفاض تدريجي في مستويات التوتر والضغط النفسي لدى المريض على مدار فترة المتابعة.

التوصيات:
- تحسين روتين النوم
- المتابعة الدورية كل أسبوعين

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', '9F442928-B53E-4FDC-B241-988969B52203', NULL, N'النسخة الأولى', '2026-03-31 12:00:00'),
('4298D6A5-1365-496D-AA8B-21A947FB389E', 'D8782544-C6AA-4374-B054-1E34A040F481', 1, N'تُظهر الجلسات الأخيرة تحسناً تدريجياً في مستوى القلق العام لدى المريض، مع استجابة جيدة لتمارين الاسترخاء والتنفس.

التوصيات:
- الاستمرار في تمارين العلاج السلوكي المعرفي (CBT)
- تحسين روتين النوم

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', '6251ABB6-B294-48CA-9B75-4E959CFA04A7', '2026-07-07 12:00:00', N'النسخة الأولى', '2026-07-06 12:00:00'),
('F1B20853-2BDF-4064-B219-65F25AB779CB', '162BDB21-6FE6-4FB2-ABBC-C415D57B7542', 1, N'تُظهر الجلسات الأخيرة تحسناً تدريجياً في مستوى القلق العام لدى المريض، مع استجابة جيدة لتمارين الاسترخاء والتنفس.

التوصيات:
- المتابعة الدورية كل أسبوعين
- زيادة النشاط الاجتماعي التدريجي

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', '6251ABB6-B294-48CA-9B75-4E959CFA04A7', NULL, N'النسخة الأولى', '2026-07-07 12:00:00'),
('106A1AD3-8764-4ACD-9569-0132995E4502', 'B0E7BA52-BC5B-4ADC-BE7C-EFE644724E1C', 1, N'تُظهر الجلسات الأخيرة تحسناً تدريجياً في مستوى القلق العام لدى المريض، مع استجابة جيدة لتمارين الاسترخاء والتنفس.

التوصيات:
- ممارسة تمارين التنفس بانتظام
- زيادة النشاط الاجتماعي التدريجي

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', '0D30427F-E161-441E-AA05-F6749620088C', '2026-06-17 12:00:00', N'النسخة الأولى', '2026-06-16 12:00:00'),
('719ED353-C6B9-47F5-920A-273B9E26073D', 'D4A7EA75-2269-4463-B524-116D12542BA7', 1, N'تُظهر الجلسات الأخيرة تحسناً تدريجياً في مستوى القلق العام لدى المريض، مع استجابة جيدة لتمارين الاسترخاء والتنفس.

التوصيات:
- الاستمرار في تمارين العلاج السلوكي المعرفي (CBT)
- زيادة النشاط الاجتماعي التدريجي

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', '0D30427F-E161-441E-AA05-F6749620088C', NULL, N'النسخة الأولى', '2026-03-16 12:00:00'),
('4124D7E2-63A6-4759-BAEF-1E24B8968F62', '91B87AB5-BC37-4D8B-A4FA-EFF6173EA91F', 1, N'تُظهر الجلسات الأخيرة تحسناً تدريجياً في مستوى القلق العام لدى المريض، مع استجابة جيدة لتمارين الاسترخاء والتنفس.

التوصيات:
- المتابعة الدورية كل أسبوعين
- زيادة النشاط الاجتماعي التدريجي

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', '25A9DCC1-6815-47F4-86AD-D25FF47B7ADE', '2026-06-30 12:00:00', N'النسخة الأولى', '2026-06-29 12:00:00'),
('38F4F9E3-C794-4EB3-8F1D-A392E514480C', '510C8B71-6877-4E6F-84AA-E917F481F113', 1, N'تُظهر الجلسات الأخيرة تحسناً تدريجياً في مستوى القلق العام لدى المريض، مع استجابة جيدة لتمارين الاسترخاء والتنفس.

التوصيات:
- ممارسة تمارين التنفس بانتظام
- الاستمرار في تمارين العلاج السلوكي المعرفي (CBT)

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', '25A9DCC1-6815-47F4-86AD-D25FF47B7ADE', NULL, N'النسخة الأولى', '2026-05-28 12:00:00'),
('05072A3D-E9B1-4881-A7B2-D49C93927B9F', '3456AD03-6B6C-48A3-AF98-53D98C25411C', 1, N'تُظهر الجلسات الأخيرة تحسناً تدريجياً في مستوى القلق العام لدى المريض، مع استجابة جيدة لتمارين الاسترخاء والتنفس.

التوصيات:
- المتابعة الدورية كل أسبوعين
- الاستمرار في تمارين العلاج السلوكي المعرفي (CBT)

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', 'DDDA597C-2D47-4C64-914D-6DF694AA3606', '2026-04-07 12:00:00', N'النسخة الأولى', '2026-04-06 12:00:00'),
('D62EE7FD-5BF9-4CCE-A865-BFE50AB26145', '242950B2-405D-4FD9-AE08-EC4E0C5F3C4E', 1, N'تُظهر الجلسات الأخيرة تحسناً تدريجياً في مستوى القلق العام لدى المريض، مع استجابة جيدة لتمارين الاسترخاء والتنفس.

التوصيات:
- ممارسة تمارين التنفس بانتظام
- المتابعة الدورية كل أسبوعين

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', 'DDDA597C-2D47-4C64-914D-6DF694AA3606', NULL, N'النسخة الأولى', '2026-03-29 12:00:00'),
('3AC98263-ED8D-4E1F-A59D-3B13311FFB90', '53233CCE-8047-470F-AB01-DA92E0046417', 1, N'تُظهر الجلسات الأخيرة تحسناً تدريجياً في مستوى القلق العام لدى المريض، مع استجابة جيدة لتمارين الاسترخاء والتنفس.

التوصيات:
- ممارسة تمارين التنفس بانتظام
- الاستمرار في تمارين العلاج السلوكي المعرفي (CBT)

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', '899B50C5-FFCF-4E6E-964D-3A59908E422E', '2026-02-26 12:00:00', N'النسخة الأولى', '2026-02-25 12:00:00'),
('85456EDF-5465-4580-8E34-760C4E4515F3', 'E5762594-8C56-41DD-A0F5-A74327F26AEB', 1, N'تُظهر الجلسات الأخيرة تحسناً تدريجياً في مستوى القلق العام لدى المريض، مع استجابة جيدة لتمارين الاسترخاء والتنفس.

التوصيات:
- المتابعة الدورية كل أسبوعين
- تحسين روتين النوم

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', '899B50C5-FFCF-4E6E-964D-3A59908E422E', NULL, N'النسخة الأولى', '2026-06-16 12:00:00'),
('8BAAA132-0A80-4C97-B77A-78E63C7CFB47', '485A62C3-9237-426A-87A7-173E6F99C1EE', 1, N'تشير المؤشرات إلى انخفاض تدريجي في مستويات التوتر والضغط النفسي لدى المريض على مدار فترة المتابعة.

التوصيات:
- المتابعة الدورية كل أسبوعين
- زيادة النشاط الاجتماعي التدريجي

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', 'E73DBDDC-8CD8-42F3-A385-E5D24710E59F', NULL, N'النسخة الأولى', '2026-04-28 12:00:00'),
('B88F0D79-E906-469E-9B1D-12496B496559', '01BA512C-A517-41E2-BB48-FF2295EFA18C', 1, N'تُظهر الجلسات الأخيرة تحسناً تدريجياً في مستوى القلق العام لدى المريض، مع استجابة جيدة لتمارين الاسترخاء والتنفس.

التوصيات:
- ممارسة تمارين التنفس بانتظام
- زيادة النشاط الاجتماعي التدريجي

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', '770DD106-350E-40A8-B1BC-4CD0BED58C40', NULL, N'النسخة الأولى', '2026-06-24 12:00:00'),
('9B682800-0146-43A4-A6B0-639DE665E6D2', '2536E371-05F2-41B5-8A64-0492D91F5A2E', 1, N'تشير المؤشرات إلى انخفاض تدريجي في مستويات التوتر والضغط النفسي لدى المريض على مدار فترة المتابعة.

التوصيات:
- زيادة النشاط الاجتماعي التدريجي
- تحسين روتين النوم

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', '9F442928-B53E-4FDC-B241-988969B52203', '2026-06-19 12:00:00', N'النسخة الأولى', '2026-06-18 12:00:00'),
('4350E2CF-8018-40E2-B15C-E81A622185E1', 'B6D5F971-A601-41E7-9B07-411316C0763D', 1, N'تشير المؤشرات إلى انخفاض تدريجي في مستويات التوتر والضغط النفسي لدى المريض على مدار فترة المتابعة.

التوصيات:
- ممارسة تمارين التنفس بانتظام
- زيادة النشاط الاجتماعي التدريجي

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', '9F442928-B53E-4FDC-B241-988969B52203', NULL, N'النسخة الأولى', '2025-11-24 12:00:00'),
('EBDF6F93-0D16-4A19-B92E-1B129B3EF23F', '1631C229-FCD6-4A13-8684-3397A758A287', 1, N'تشير المؤشرات إلى انخفاض تدريجي في مستويات التوتر والضغط النفسي لدى المريض على مدار فترة المتابعة.

التوصيات:
- الاستمرار في تمارين العلاج السلوكي المعرفي (CBT)
- المتابعة الدورية كل أسبوعين

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', '6251ABB6-B294-48CA-9B75-4E959CFA04A7', '2026-06-10 12:00:00', N'النسخة الأولى', '2026-06-09 12:00:00'),
('136DD374-065D-4602-AA17-84E3D8B4BC5B', '81B8ED6A-0DD5-473F-A952-C60258EA73CE', 1, N'تشير المؤشرات إلى انخفاض تدريجي في مستويات التوتر والضغط النفسي لدى المريض على مدار فترة المتابعة.

التوصيات:
- زيادة النشاط الاجتماعي التدريجي
- تحسين روتين النوم

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', '6251ABB6-B294-48CA-9B75-4E959CFA04A7', NULL, N'النسخة الأولى', '2025-11-28 12:00:00'),
('63632014-3EA2-4129-9593-FC4B705E2378', '143D58A2-DB82-4082-8349-DD7D2B9B48CC', 1, N'يُظهر المريض تحسناً تدريجياً في المزاج العام ومستوى الطاقة مقارنة ببداية العلاج، مع استمرار بعض أعراض الاكتئاب الخفيفة.

التوصيات:
- تحسين روتين النوم
- زيادة النشاط الاجتماعي التدريجي

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', '0D30427F-E161-441E-AA05-F6749620088C', NULL, N'النسخة الأولى', '2026-05-25 12:00:00'),
('45D3E173-2085-45F4-9EC4-5C4A2DF041EC', '27E7CB86-589F-46D4-B1AC-B151CE267968', 1, N'تشير المؤشرات إلى انخفاض تدريجي في مستويات التوتر والضغط النفسي لدى المريض على مدار فترة المتابعة.

التوصيات:
- ممارسة تمارين التنفس بانتظام
- الاستمرار في تمارين العلاج السلوكي المعرفي (CBT)

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', '25A9DCC1-6815-47F4-86AD-D25FF47B7ADE', NULL, N'النسخة الأولى', '2026-07-03 12:00:00'),
('5F50CBC5-74C8-492D-9F3E-AB4D16365C00', 'A645A80E-1DCC-47E4-BCF8-5C1B7E0ADAF7', 1, N'تشير المؤشرات إلى انخفاض تدريجي في مستويات التوتر والضغط النفسي لدى المريض على مدار فترة المتابعة.

التوصيات:
- المتابعة الدورية كل أسبوعين
- الاستمرار في تمارين العلاج السلوكي المعرفي (CBT)

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', 'DDDA597C-2D47-4C64-914D-6DF694AA3606', '2026-02-18 12:00:00', N'النسخة الأولى', '2026-02-17 12:00:00'),
('4C541ECB-FA0C-49F4-B7D7-6C0823EE3CAD', 'B4E0D6DA-2D97-4C63-9EA4-5B2025144B2F', 1, N'تشير المؤشرات إلى انخفاض تدريجي في مستويات التوتر والضغط النفسي لدى المريض على مدار فترة المتابعة.

التوصيات:
- ممارسة تمارين التنفس بانتظام
- الاستمرار في تمارين العلاج السلوكي المعرفي (CBT)

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', 'DDDA597C-2D47-4C64-914D-6DF694AA3606', NULL, N'النسخة الأولى', '2026-02-09 12:00:00'),
('263FD1ED-D830-4C1B-9EEC-D55A8881921E', '66E238E6-AB5C-4379-A200-8D79516CDBF7', 1, N'تشير المؤشرات إلى انخفاض تدريجي في مستويات التوتر والضغط النفسي لدى المريض على مدار فترة المتابعة.

التوصيات:
- زيادة النشاط الاجتماعي التدريجي
- ممارسة تمارين التنفس بانتظام

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', '899B50C5-FFCF-4E6E-964D-3A59908E422E', '2025-12-08 12:00:00', N'النسخة الأولى', '2025-12-07 12:00:00'),
('538509CE-1CDF-4834-8327-D19EECD5D9C8', 'F929250B-89D0-46EA-9781-4A199C59F8F6', 1, N'تشير المؤشرات إلى انخفاض تدريجي في مستويات التوتر والضغط النفسي لدى المريض على مدار فترة المتابعة.

التوصيات:
- المتابعة الدورية كل أسبوعين
- الاستمرار في تمارين العلاج السلوكي المعرفي (CBT)

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', '899B50C5-FFCF-4E6E-964D-3A59908E422E', NULL, N'النسخة الأولى', '2026-03-16 12:00:00'),
('4E6C84CC-C6F2-46D8-A5C3-3A72F4D4BF26', '9C2CAE46-BEAA-4621-9474-8EFFA8D16289', 1, N'تشير المؤشرات إلى انخفاض تدريجي في مستويات التوتر والضغط النفسي لدى المريض على مدار فترة المتابعة.

التوصيات:
- الاستمرار في تمارين العلاج السلوكي المعرفي (CBT)
- تحسين روتين النوم

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', 'E73DBDDC-8CD8-42F3-A385-E5D24710E59F', NULL, N'النسخة الأولى', '2026-04-11 12:00:00'),
('4FE5524A-FB80-47B5-AC21-FDCEDD5818A5', '971B3A36-5EBA-4921-939F-DA23E52698F4', 1, N'يُظهر المريض تحسناً تدريجياً في المزاج العام ومستوى الطاقة مقارنة ببداية العلاج، مع استمرار بعض أعراض الاكتئاب الخفيفة.

التوصيات:
- زيادة النشاط الاجتماعي التدريجي
- ممارسة تمارين التنفس بانتظام

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', '770DD106-350E-40A8-B1BC-4CD0BED58C40', '2026-07-19 12:00:00', N'النسخة الأولى', '2026-07-18 12:00:00'),
('44A77B32-40CD-4AF3-A2BD-400EE360D2A4', '36CDF84D-A8D6-4D7C-AB08-CB96F6CFB3F3', 1, N'يُظهر المريض تحسناً تدريجياً في المزاج العام ومستوى الطاقة مقارنة ببداية العلاج، مع استمرار بعض أعراض الاكتئاب الخفيفة.

التوصيات:
- ممارسة تمارين التنفس بانتظام
- زيادة النشاط الاجتماعي التدريجي

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', '770DD106-350E-40A8-B1BC-4CD0BED58C40', NULL, N'النسخة الأولى', '2026-07-18 12:00:00'),
('B12C411B-4706-47A5-80EA-79B76982EAB1', '0F2C1F23-7820-4C90-9FBE-6A53C21F70B3', 1, N'تشير المؤشرات إلى انخفاض تدريجي في مستويات التوتر والضغط النفسي لدى المريض على مدار فترة المتابعة.

التوصيات:
- ممارسة تمارين التنفس بانتظام
- تحسين روتين النوم

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', '9F442928-B53E-4FDC-B241-988969B52203', '2026-05-09 12:00:00', N'النسخة الأولى', '2026-05-08 12:00:00'),
('BAAC8B59-3612-4173-A2E5-941A1307E021', '1A437683-F6C2-4148-985C-B48544E08DA4', 1, N'تشير المؤشرات إلى انخفاض تدريجي في مستويات التوتر والضغط النفسي لدى المريض على مدار فترة المتابعة.

التوصيات:
- المتابعة الدورية كل أسبوعين
- تحسين روتين النوم

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', '9F442928-B53E-4FDC-B241-988969B52203', NULL, N'النسخة الأولى', '2026-02-26 12:00:00'),
('612AFDAD-2522-43D2-B159-4B1D30E9BD2A', 'BE699D36-503B-4100-BA05-AE63DB4D2D2A', 1, N'تُظهر الجلسات الأخيرة تحسناً تدريجياً في مستوى القلق العام لدى المريض، مع استجابة جيدة لتمارين الاسترخاء والتنفس.

التوصيات:
- زيادة النشاط الاجتماعي التدريجي
- المتابعة الدورية كل أسبوعين

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', '6251ABB6-B294-48CA-9B75-4E959CFA04A7', '2026-07-12 12:00:00', N'النسخة الأولى', '2026-07-11 12:00:00'),
('2A77A129-DAAB-492F-8DFE-942F561CE725', 'BF1CE234-9C8A-4A2C-A5B7-5A56573F9E16', 1, N'تُظهر الجلسات الأخيرة تحسناً تدريجياً في مستوى القلق العام لدى المريض، مع استجابة جيدة لتمارين الاسترخاء والتنفس.

التوصيات:
- زيادة النشاط الاجتماعي التدريجي
- تحسين روتين النوم

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', '6251ABB6-B294-48CA-9B75-4E959CFA04A7', NULL, N'النسخة الأولى', '2026-07-11 12:00:00'),
('F39A686A-C639-424D-9E9B-E6612680B961', '2672DDC7-E1D5-4625-9712-1D54F1DC0AD8', 1, N'يُظهر المريض تحسناً تدريجياً في المزاج العام ومستوى الطاقة مقارنة ببداية العلاج، مع استمرار بعض أعراض الاكتئاب الخفيفة.

التوصيات:
- الاستمرار في تمارين العلاج السلوكي المعرفي (CBT)
- المتابعة الدورية كل أسبوعين

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', '0D30427F-E161-441E-AA05-F6749620088C', '2026-04-02 12:00:00', N'النسخة الأولى', '2026-04-01 12:00:00'),
('EA6956B1-69B9-445A-84D7-00296397E9B0', '40A0AD1E-96FC-4314-8A24-91A989A2EAA5', 1, N'يُظهر المريض تحسناً تدريجياً في المزاج العام ومستوى الطاقة مقارنة ببداية العلاج، مع استمرار بعض أعراض الاكتئاب الخفيفة.

التوصيات:
- الاستمرار في تمارين العلاج السلوكي المعرفي (CBT)
- زيادة النشاط الاجتماعي التدريجي

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', '0D30427F-E161-441E-AA05-F6749620088C', NULL, N'النسخة الأولى', '2026-05-31 12:00:00'),
('16528B7C-64AE-4C73-A1C5-1B38DEF1396B', 'F523819E-1302-441C-95C4-4DA2138F560A', 1, N'تشير المؤشرات إلى انخفاض تدريجي في مستويات التوتر والضغط النفسي لدى المريض على مدار فترة المتابعة.

التوصيات:
- المتابعة الدورية كل أسبوعين
- الاستمرار في تمارين العلاج السلوكي المعرفي (CBT)

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', '25A9DCC1-6815-47F4-86AD-D25FF47B7ADE', NULL, N'النسخة الأولى', '2026-03-04 12:00:00'),
('40F3C249-5101-4FE3-8054-7EFE8C90D1FB', '454EAE95-4D2D-48AA-9BD4-9FD089BF1E3E', 1, N'تشير المؤشرات إلى انخفاض تدريجي في مستويات التوتر والضغط النفسي لدى المريض على مدار فترة المتابعة.

التوصيات:
- الاستمرار في تمارين العلاج السلوكي المعرفي (CBT)
- تحسين روتين النوم

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', 'DDDA597C-2D47-4C64-914D-6DF694AA3606', '2026-07-03 12:00:00', N'النسخة الأولى', '2026-07-02 12:00:00'),
('6FE71EC8-AE74-45DE-A6E6-C1BC0E5523B5', '95C4470D-DEB7-4903-9839-397BF73D8326', 1, N'تشير المؤشرات إلى انخفاض تدريجي في مستويات التوتر والضغط النفسي لدى المريض على مدار فترة المتابعة.

التوصيات:
- تحسين روتين النوم
- الاستمرار في تمارين العلاج السلوكي المعرفي (CBT)

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', 'DDDA597C-2D47-4C64-914D-6DF694AA3606', NULL, N'النسخة الأولى', '2026-05-23 12:00:00'),
('0C8C4F47-FF5F-4299-B3EE-D175917A1A58', '4B6CC5D7-410D-4AD1-9EC5-8577AB00F703', 1, N'تُظهر الجلسات الأخيرة تحسناً تدريجياً في مستوى القلق العام لدى المريض، مع استجابة جيدة لتمارين الاسترخاء والتنفس.

التوصيات:
- المتابعة الدورية كل أسبوعين
- ممارسة تمارين التنفس بانتظام

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', '899B50C5-FFCF-4E6E-964D-3A59908E422E', '2026-06-25 12:00:00', N'النسخة الأولى', '2026-06-24 12:00:00'),
('BC4DB3ED-D330-4613-9601-1501CA69BE70', '5F99E030-A134-4727-88A8-345EB021F6DB', 1, N'تُظهر الجلسات الأخيرة تحسناً تدريجياً في مستوى القلق العام لدى المريض، مع استجابة جيدة لتمارين الاسترخاء والتنفس.

التوصيات:
- تحسين روتين النوم
- زيادة النشاط الاجتماعي التدريجي

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', '899B50C5-FFCF-4E6E-964D-3A59908E422E', NULL, N'النسخة الأولى', '2026-06-29 12:00:00'),
('A0870C34-F293-4C1A-8437-2FB5B3B6D52F', '92D82131-9AFA-4F0B-99FC-3ED47BB72F19', 1, N'تُظهر الجلسات الأخيرة تحسناً تدريجياً في مستوى القلق العام لدى المريض، مع استجابة جيدة لتمارين الاسترخاء والتنفس.

التوصيات:
- المتابعة الدورية كل أسبوعين
- ممارسة تمارين التنفس بانتظام

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', 'E73DBDDC-8CD8-42F3-A385-E5D24710E59F', NULL, N'النسخة الأولى', '2026-04-16 12:00:00'),
('7A398E53-3845-4FBD-9C82-072E4B0C3BBD', 'C5543C2E-4379-4B50-B811-869B705C85C7', 1, N'يُظهر المريض تحسناً تدريجياً في المزاج العام ومستوى الطاقة مقارنة ببداية العلاج، مع استمرار بعض أعراض الاكتئاب الخفيفة.

التوصيات:
- ممارسة تمارين التنفس بانتظام
- زيادة النشاط الاجتماعي التدريجي

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', '770DD106-350E-40A8-B1BC-4CD0BED58C40', NULL, N'النسخة الأولى', '2026-06-26 12:00:00'),
('698F4E76-9389-4BE4-B426-A58D4F0C68A5', '3866A896-C50F-42FD-92AC-89371C5BB378', 1, N'تُظهر الجلسات الأخيرة تحسناً تدريجياً في مستوى القلق العام لدى المريض، مع استجابة جيدة لتمارين الاسترخاء والتنفس.

التوصيات:
- المتابعة الدورية كل أسبوعين
- زيادة النشاط الاجتماعي التدريجي

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', '9F442928-B53E-4FDC-B241-988969B52203', '2026-07-02 12:00:00', N'النسخة الأولى', '2026-07-01 12:00:00'),
('F439767D-75A4-4522-B35C-974993330837', 'A125E586-DA08-40AA-AF94-CD4559A75D22', 1, N'تُظهر الجلسات الأخيرة تحسناً تدريجياً في مستوى القلق العام لدى المريض، مع استجابة جيدة لتمارين الاسترخاء والتنفس.

التوصيات:
- زيادة النشاط الاجتماعي التدريجي
- المتابعة الدورية كل أسبوعين

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', '9F442928-B53E-4FDC-B241-988969B52203', NULL, N'النسخة الأولى', '2026-06-06 12:00:00'),
('607B780D-A537-4D61-8C98-2A114554447A', '6F314E07-F840-48AA-89BB-2C184AA9F6F4', 1, N'تُظهر الجلسات الأخيرة تحسناً تدريجياً في مستوى القلق العام لدى المريض، مع استجابة جيدة لتمارين الاسترخاء والتنفس.

التوصيات:
- الاستمرار في تمارين العلاج السلوكي المعرفي (CBT)
- المتابعة الدورية كل أسبوعين

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', '6251ABB6-B294-48CA-9B75-4E959CFA04A7', NULL, N'النسخة الأولى', '2026-05-30 12:00:00'),
('F1A1DDF0-DA55-413A-A89D-09049F75A52C', '70561C73-88AC-4F91-BE4C-52562AEC600D', 1, N'تُظهر الجلسات الأخيرة تحسناً تدريجياً في مستوى القلق العام لدى المريض، مع استجابة جيدة لتمارين الاسترخاء والتنفس.

التوصيات:
- ممارسة تمارين التنفس بانتظام
- زيادة النشاط الاجتماعي التدريجي

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', '0D30427F-E161-441E-AA05-F6749620088C', '2026-05-08 12:00:00', N'النسخة الأولى', '2026-05-07 12:00:00'),
('64592E8F-A356-4A6C-A669-4A63FE2C3CB2', '4B759911-A29D-48C3-A62F-188F48AC5823', 1, N'تُظهر الجلسات الأخيرة تحسناً تدريجياً في مستوى القلق العام لدى المريض، مع استجابة جيدة لتمارين الاسترخاء والتنفس.

التوصيات:
- زيادة النشاط الاجتماعي التدريجي
- تحسين روتين النوم

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', '0D30427F-E161-441E-AA05-F6749620088C', NULL, N'النسخة الأولى', '2026-06-15 12:00:00'),
('49169504-2A64-4D37-93D0-FC1BDCF50735', '14F80424-06AD-404B-83FC-EE7DE475DACE', 1, N'تُظهر الجلسات الأخيرة تحسناً تدريجياً في مستوى القلق العام لدى المريض، مع استجابة جيدة لتمارين الاسترخاء والتنفس.

التوصيات:
- المتابعة الدورية كل أسبوعين
- زيادة النشاط الاجتماعي التدريجي

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', '25A9DCC1-6815-47F4-86AD-D25FF47B7ADE', '2026-03-14 12:00:00', N'النسخة الأولى', '2026-03-13 12:00:00'),
('F4D6940E-7DED-4B50-9113-CB1EEE20D0D7', '9E7DECB1-2A41-4087-B4FF-7A8F7D802900', 1, N'تُظهر الجلسات الأخيرة تحسناً تدريجياً في مستوى القلق العام لدى المريض، مع استجابة جيدة لتمارين الاسترخاء والتنفس.

التوصيات:
- الاستمرار في تمارين العلاج السلوكي المعرفي (CBT)
- زيادة النشاط الاجتماعي التدريجي

ملاحظات المعالج:
المريض يظهر التزاماً جيداً بالخطة العلاجية ويُنصح بمتابعة التقدم بشكل شهري مع إعادة تقييم دوري.', '25A9DCC1-6815-47F4-86AD-D25FF47B7ADE', NULL, N'النسخة الأولى', '2025-10-02 12:00:00');

-- ===== Link ReferralReports.CurrentVersionId -> ReportVersions.Id =====
UPDATE [ReferralReports] SET [CurrentVersionId] = 'CC3CC2E9-EF69-4455-9731-43FC5802CAC9' WHERE [Id] = '9037759B-DD09-4E05-93DE-EEFDC968183A';
UPDATE [ReferralReports] SET [CurrentVersionId] = 'B6D1592E-8CAB-444F-AB3E-D37230098823' WHERE [Id] = '50A7CB45-0345-437C-AC52-5049A91B5C8B';
UPDATE [ReferralReports] SET [CurrentVersionId] = '0EA35E8D-31FB-4327-8DB1-F71CEED07E95' WHERE [Id] = '7FB31C31-E0F5-40FC-9BB9-7EA1C9B2949A';
UPDATE [ReferralReports] SET [CurrentVersionId] = '6D01861B-D498-4C05-A707-7AEFAC468341' WHERE [Id] = '5D59D23C-1A81-48EC-85B1-F412BAAC78FC';
UPDATE [ReferralReports] SET [CurrentVersionId] = '0136092F-B742-4C6D-B493-E676EF679BFC' WHERE [Id] = '88836645-F9A6-4988-9CFC-9107C85761E5';
UPDATE [ReferralReports] SET [CurrentVersionId] = '49750132-AA60-4176-A8B5-6A0324C9B264' WHERE [Id] = 'FED06835-0677-457B-BA44-8FA599F96551';
UPDATE [ReferralReports] SET [CurrentVersionId] = '46D775BC-3F44-4319-BEDC-E33B98074E6A' WHERE [Id] = '0521C9DF-D099-4D9B-9AA6-0B5D8D3295BA';
UPDATE [ReferralReports] SET [CurrentVersionId] = '7DDFF79D-FD33-4827-B42E-2DD3AE2E8761' WHERE [Id] = '011FB171-19B5-4F5C-9827-245749DAAD9E';
UPDATE [ReferralReports] SET [CurrentVersionId] = '42B8B0A1-C717-4F3C-A725-26AD46A00EEC' WHERE [Id] = '340B9B99-805C-4E87-A154-7F5EB394C69B';
UPDATE [ReferralReports] SET [CurrentVersionId] = 'E8B1CE59-BDD2-4C31-BA4A-57931FAFBD52' WHERE [Id] = 'C866085E-955E-469F-8F0E-AB148E31852F';
UPDATE [ReferralReports] SET [CurrentVersionId] = '4298D6A5-1365-496D-AA8B-21A947FB389E' WHERE [Id] = 'D8782544-C6AA-4374-B054-1E34A040F481';
UPDATE [ReferralReports] SET [CurrentVersionId] = 'F1B20853-2BDF-4064-B219-65F25AB779CB' WHERE [Id] = '162BDB21-6FE6-4FB2-ABBC-C415D57B7542';
UPDATE [ReferralReports] SET [CurrentVersionId] = '106A1AD3-8764-4ACD-9569-0132995E4502' WHERE [Id] = 'B0E7BA52-BC5B-4ADC-BE7C-EFE644724E1C';
UPDATE [ReferralReports] SET [CurrentVersionId] = '719ED353-C6B9-47F5-920A-273B9E26073D' WHERE [Id] = 'D4A7EA75-2269-4463-B524-116D12542BA7';
UPDATE [ReferralReports] SET [CurrentVersionId] = '4124D7E2-63A6-4759-BAEF-1E24B8968F62' WHERE [Id] = '91B87AB5-BC37-4D8B-A4FA-EFF6173EA91F';
UPDATE [ReferralReports] SET [CurrentVersionId] = '38F4F9E3-C794-4EB3-8F1D-A392E514480C' WHERE [Id] = '510C8B71-6877-4E6F-84AA-E917F481F113';
UPDATE [ReferralReports] SET [CurrentVersionId] = '05072A3D-E9B1-4881-A7B2-D49C93927B9F' WHERE [Id] = '3456AD03-6B6C-48A3-AF98-53D98C25411C';
UPDATE [ReferralReports] SET [CurrentVersionId] = 'D62EE7FD-5BF9-4CCE-A865-BFE50AB26145' WHERE [Id] = '242950B2-405D-4FD9-AE08-EC4E0C5F3C4E';
UPDATE [ReferralReports] SET [CurrentVersionId] = '3AC98263-ED8D-4E1F-A59D-3B13311FFB90' WHERE [Id] = '53233CCE-8047-470F-AB01-DA92E0046417';
UPDATE [ReferralReports] SET [CurrentVersionId] = '85456EDF-5465-4580-8E34-760C4E4515F3' WHERE [Id] = 'E5762594-8C56-41DD-A0F5-A74327F26AEB';
UPDATE [ReferralReports] SET [CurrentVersionId] = '8BAAA132-0A80-4C97-B77A-78E63C7CFB47' WHERE [Id] = '485A62C3-9237-426A-87A7-173E6F99C1EE';
UPDATE [ReferralReports] SET [CurrentVersionId] = 'B88F0D79-E906-469E-9B1D-12496B496559' WHERE [Id] = '01BA512C-A517-41E2-BB48-FF2295EFA18C';
UPDATE [ReferralReports] SET [CurrentVersionId] = '9B682800-0146-43A4-A6B0-639DE665E6D2' WHERE [Id] = '2536E371-05F2-41B5-8A64-0492D91F5A2E';
UPDATE [ReferralReports] SET [CurrentVersionId] = '4350E2CF-8018-40E2-B15C-E81A622185E1' WHERE [Id] = 'B6D5F971-A601-41E7-9B07-411316C0763D';
UPDATE [ReferralReports] SET [CurrentVersionId] = 'EBDF6F93-0D16-4A19-B92E-1B129B3EF23F' WHERE [Id] = '1631C229-FCD6-4A13-8684-3397A758A287';
UPDATE [ReferralReports] SET [CurrentVersionId] = '136DD374-065D-4602-AA17-84E3D8B4BC5B' WHERE [Id] = '81B8ED6A-0DD5-473F-A952-C60258EA73CE';
UPDATE [ReferralReports] SET [CurrentVersionId] = '63632014-3EA2-4129-9593-FC4B705E2378' WHERE [Id] = '143D58A2-DB82-4082-8349-DD7D2B9B48CC';
UPDATE [ReferralReports] SET [CurrentVersionId] = '45D3E173-2085-45F4-9EC4-5C4A2DF041EC' WHERE [Id] = '27E7CB86-589F-46D4-B1AC-B151CE267968';
UPDATE [ReferralReports] SET [CurrentVersionId] = '5F50CBC5-74C8-492D-9F3E-AB4D16365C00' WHERE [Id] = 'A645A80E-1DCC-47E4-BCF8-5C1B7E0ADAF7';
UPDATE [ReferralReports] SET [CurrentVersionId] = '4C541ECB-FA0C-49F4-B7D7-6C0823EE3CAD' WHERE [Id] = 'B4E0D6DA-2D97-4C63-9EA4-5B2025144B2F';
UPDATE [ReferralReports] SET [CurrentVersionId] = '263FD1ED-D830-4C1B-9EEC-D55A8881921E' WHERE [Id] = '66E238E6-AB5C-4379-A200-8D79516CDBF7';
UPDATE [ReferralReports] SET [CurrentVersionId] = '538509CE-1CDF-4834-8327-D19EECD5D9C8' WHERE [Id] = 'F929250B-89D0-46EA-9781-4A199C59F8F6';
UPDATE [ReferralReports] SET [CurrentVersionId] = '4E6C84CC-C6F2-46D8-A5C3-3A72F4D4BF26' WHERE [Id] = '9C2CAE46-BEAA-4621-9474-8EFFA8D16289';
UPDATE [ReferralReports] SET [CurrentVersionId] = '4FE5524A-FB80-47B5-AC21-FDCEDD5818A5' WHERE [Id] = '971B3A36-5EBA-4921-939F-DA23E52698F4';
UPDATE [ReferralReports] SET [CurrentVersionId] = '44A77B32-40CD-4AF3-A2BD-400EE360D2A4' WHERE [Id] = '36CDF84D-A8D6-4D7C-AB08-CB96F6CFB3F3';
UPDATE [ReferralReports] SET [CurrentVersionId] = 'B12C411B-4706-47A5-80EA-79B76982EAB1' WHERE [Id] = '0F2C1F23-7820-4C90-9FBE-6A53C21F70B3';
UPDATE [ReferralReports] SET [CurrentVersionId] = 'BAAC8B59-3612-4173-A2E5-941A1307E021' WHERE [Id] = '1A437683-F6C2-4148-985C-B48544E08DA4';
UPDATE [ReferralReports] SET [CurrentVersionId] = '612AFDAD-2522-43D2-B159-4B1D30E9BD2A' WHERE [Id] = 'BE699D36-503B-4100-BA05-AE63DB4D2D2A';
UPDATE [ReferralReports] SET [CurrentVersionId] = '2A77A129-DAAB-492F-8DFE-942F561CE725' WHERE [Id] = 'BF1CE234-9C8A-4A2C-A5B7-5A56573F9E16';
UPDATE [ReferralReports] SET [CurrentVersionId] = 'F39A686A-C639-424D-9E9B-E6612680B961' WHERE [Id] = '2672DDC7-E1D5-4625-9712-1D54F1DC0AD8';
UPDATE [ReferralReports] SET [CurrentVersionId] = 'EA6956B1-69B9-445A-84D7-00296397E9B0' WHERE [Id] = '40A0AD1E-96FC-4314-8A24-91A989A2EAA5';
UPDATE [ReferralReports] SET [CurrentVersionId] = '16528B7C-64AE-4C73-A1C5-1B38DEF1396B' WHERE [Id] = 'F523819E-1302-441C-95C4-4DA2138F560A';
UPDATE [ReferralReports] SET [CurrentVersionId] = '40F3C249-5101-4FE3-8054-7EFE8C90D1FB' WHERE [Id] = '454EAE95-4D2D-48AA-9BD4-9FD089BF1E3E';
UPDATE [ReferralReports] SET [CurrentVersionId] = '6FE71EC8-AE74-45DE-A6E6-C1BC0E5523B5' WHERE [Id] = '95C4470D-DEB7-4903-9839-397BF73D8326';
UPDATE [ReferralReports] SET [CurrentVersionId] = '0C8C4F47-FF5F-4299-B3EE-D175917A1A58' WHERE [Id] = '4B6CC5D7-410D-4AD1-9EC5-8577AB00F703';
UPDATE [ReferralReports] SET [CurrentVersionId] = 'BC4DB3ED-D330-4613-9601-1501CA69BE70' WHERE [Id] = '5F99E030-A134-4727-88A8-345EB021F6DB';
UPDATE [ReferralReports] SET [CurrentVersionId] = 'A0870C34-F293-4C1A-8437-2FB5B3B6D52F' WHERE [Id] = '92D82131-9AFA-4F0B-99FC-3ED47BB72F19';
UPDATE [ReferralReports] SET [CurrentVersionId] = '7A398E53-3845-4FBD-9C82-072E4B0C3BBD' WHERE [Id] = 'C5543C2E-4379-4B50-B811-869B705C85C7';
UPDATE [ReferralReports] SET [CurrentVersionId] = '698F4E76-9389-4BE4-B426-A58D4F0C68A5' WHERE [Id] = '3866A896-C50F-42FD-92AC-89371C5BB378';
UPDATE [ReferralReports] SET [CurrentVersionId] = 'F439767D-75A4-4522-B35C-974993330837' WHERE [Id] = 'A125E586-DA08-40AA-AF94-CD4559A75D22';
UPDATE [ReferralReports] SET [CurrentVersionId] = '607B780D-A537-4D61-8C98-2A114554447A' WHERE [Id] = '6F314E07-F840-48AA-89BB-2C184AA9F6F4';
UPDATE [ReferralReports] SET [CurrentVersionId] = 'F1A1DDF0-DA55-413A-A89D-09049F75A52C' WHERE [Id] = '70561C73-88AC-4F91-BE4C-52562AEC600D';
UPDATE [ReferralReports] SET [CurrentVersionId] = '64592E8F-A356-4A6C-A669-4A63FE2C3CB2' WHERE [Id] = '4B759911-A29D-48C3-A62F-188F48AC5823';
UPDATE [ReferralReports] SET [CurrentVersionId] = '49169504-2A64-4D37-93D0-FC1BDCF50735' WHERE [Id] = '14F80424-06AD-404B-83FC-EE7DE475DACE';
UPDATE [ReferralReports] SET [CurrentVersionId] = 'F4D6940E-7DED-4B50-9113-CB1EEE20D0D7' WHERE [Id] = '9E7DECB1-2A41-4087-B4FF-7A8F7D802900';

-- ===== CrisisAlerts =====
INSERT INTO [CrisisAlerts] ([Id], [PatientId], [TherapistId], [Severity], [ChatMessageId], [Status], [CreatedAt], [UpdatedAt])
VALUES
('E1616311-F03F-4C06-B1CC-BAB9E800152B', '5947B1E9-EACB-4CD1-A014-69DBDA364362', 'DDDA597C-2D47-4C64-914D-6DF694AA3606', N'High', NULL, N'Open', '2026-06-24 12:00:00', '2026-06-24 12:00:00'),
('98B62E34-DDB7-4C49-B8B8-2486057DFDA4', 'F300A791-B73E-4A86-90EA-4D1EE67BBE48', '899B50C5-FFCF-4E6E-964D-3A59908E422E', N'High', NULL, N'Open', '2026-07-05 12:00:00', '2026-07-05 12:00:00'),
('127CF1DC-2E30-4393-948F-A8D8F5FAF113', '69F104EE-9B40-40CF-A029-EC2A532693FA', 'E73DBDDC-8CD8-42F3-A385-E5D24710E59F', N'Medium', NULL, N'Resolved', '2026-06-17 12:00:00', '2026-06-17 12:00:00'),
('C0D6951C-2D77-4079-B3A7-038ECC793C33', '74FDAC62-9C1A-472B-AA93-BCDC4103514D', 'E73DBDDC-8CD8-42F3-A385-E5D24710E59F', N'Critical', NULL, N'Resolved', '2026-06-13 12:00:00', '2026-06-13 12:00:00'),
('3C4350C1-077D-4DD2-89FC-05559EAE9687', '7F9C808B-66FB-44EC-AE2F-D987CCB0D98A', 'E73DBDDC-8CD8-42F3-A385-E5D24710E59F', N'Medium', NULL, N'Resolved', '2026-06-12 12:00:00', '2026-06-12 12:00:00'),
('329AE95B-5BA0-40B1-9412-C3643379C7CA', '85C8A7BC-BBE9-4D03-B8D2-89AFEE304102', '25A9DCC1-6815-47F4-86AD-D25FF47B7ADE', N'Medium', NULL, N'Acknowledged', '2026-07-02 12:00:00', '2026-07-02 12:00:00');

-- ===== Notifications =====
INSERT INTO [Notifications] ([Id], [RecipientUserId], [Type], [Title], [Body], [IsRead], [ReadAt], [CreatedAt])
VALUES
('D92BDE1D-D4F2-40DB-BEFD-059EDE854C00', '2D19FD6D-560A-451B-BC1F-4E446A0749B1', N'SystemReport', N'تقرير النظام الأسبوعي', N'ملخص نشاط النظام الأسبوعي جاهز للمراجعة.', 0, NULL, '2026-06-06 12:00:00'),
('20E2FD31-DB04-43CC-AE90-034DC0CCB224', '2D19FD6D-560A-451B-BC1F-4E446A0749B1', N'SystemReport', N'تقرير النظام الأسبوعي', N'ملخص نشاط النظام الأسبوعي جاهز للمراجعة.', 1, '2026-06-28 18:06:00', '2026-06-28 12:00:00'),
('83B09931-8A3B-492D-8605-F30416B82E25', '2D19FD6D-560A-451B-BC1F-4E446A0749B1', N'NewUser', N'تسجيل مستخدم جديد', N'تم تسجيل معالج جديد في النظام.', 1, '2026-05-27 18:18:00', '2026-05-27 12:00:00'),
('B1DB7214-616D-4751-902F-A468C6912BA2', '2D19FD6D-560A-451B-BC1F-4E446A0749B1', N'NewUser', N'تسجيل مستخدم جديد', N'تم تسجيل معالج جديد في النظام.', 1, '2026-06-27 19:11:00', '2026-06-27 12:00:00'),
('758C2A95-380C-46CB-9E68-7D9DE09A0383', '2D19FD6D-560A-451B-BC1F-4E446A0749B1', N'SystemReport', N'تقرير النظام الأسبوعي', N'ملخص نشاط النظام الأسبوعي جاهز للمراجعة.', 1, '2026-06-28 15:08:00', '2026-06-28 12:00:00'),
('00C08714-19F2-49DD-AF85-DDFE8D466A8B', '2D19FD6D-560A-451B-BC1F-4E446A0749B1', N'SystemReport', N'تقرير النظام الأسبوعي', N'ملخص نشاط النظام الأسبوعي جاهز للمراجعة.', 0, NULL, '2026-06-02 12:00:00'),
('9B886627-2210-432C-B727-519284D7EA10', '2D19FD6D-560A-451B-BC1F-4E446A0749B1', N'NewUser', N'تسجيل مستخدم جديد', N'تم تسجيل معالج جديد في النظام.', 1, '2026-06-07 16:28:00', '2026-06-07 12:00:00'),
('7E87760A-A17F-421A-9736-D4BB56509005', '8707DD3C-5834-4909-81E1-69CFA4AB12B5', N'CrisisAlert', N'تنبيه أزمة', N'تم رصد مؤشر أزمة محتمل لدى أحد المرضى، يرجى المتابعة الفورية.', 1, '2026-06-15 18:50:00', '2026-06-15 12:00:00'),
('A996FEDF-7BB7-4963-9BC5-A80BA4AAFAA4', '8707DD3C-5834-4909-81E1-69CFA4AB12B5', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة مجدولة مع أحد المرضى قريباً.', 1, '2026-06-03 12:32:00', '2026-06-03 12:00:00'),
('C9ADB8EC-1027-417C-AF5E-14091C2BA5CF', '8707DD3C-5834-4909-81E1-69CFA4AB12B5', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة مجدولة مع أحد المرضى قريباً.', 1, '2026-05-22 17:59:00', '2026-05-22 12:00:00'),
('D9B3D688-8310-427D-B5A2-B83345C1557A', '8707DD3C-5834-4909-81E1-69CFA4AB12B5', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة مجدولة مع أحد المرضى قريباً.', 1, '2026-06-16 19:32:00', '2026-06-16 12:00:00'),
('C5FD30C3-C92C-40E6-921A-0A83B7B56109', '8707DD3C-5834-4909-81E1-69CFA4AB12B5', N'CrisisAlert', N'تنبيه أزمة', N'تم رصد مؤشر أزمة محتمل لدى أحد المرضى، يرجى المتابعة الفورية.', 0, NULL, '2026-05-25 12:00:00'),
('71CD098C-562D-476C-9F12-CBDF586306D0', '8707DD3C-5834-4909-81E1-69CFA4AB12B5', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة مجدولة مع أحد المرضى قريباً.', 1, '2026-07-04 13:36:00', '2026-07-04 12:00:00'),
('40550B65-0F06-435F-A0D2-366A087A62F0', '8707DD3C-5834-4909-81E1-69CFA4AB12B5', N'WeeklyProgressReport', N'تقرير جاهز للمراجعة', N'تم إنشاء تقرير إحالة جديد بانتظار مراجعتك واعتماده.', 0, NULL, '2026-06-04 12:00:00'),
('624E1A6E-579F-49B1-B163-831761CC4095', '8707DD3C-5834-4909-81E1-69CFA4AB12B5', N'WeeklyProgressReport', N'تقرير جاهز للمراجعة', N'تم إنشاء تقرير إحالة جديد بانتظار مراجعتك واعتماده.', 1, '2026-06-11 14:18:00', '2026-06-11 12:00:00'),
('B86E4E6A-8089-435D-80C2-1DFD342276A3', 'D13A0975-0432-4FAA-86BB-8CE87DC80AD3', N'WeeklyProgressReport', N'تقرير جاهز للمراجعة', N'تم إنشاء تقرير إحالة جديد بانتظار مراجعتك واعتماده.', 1, '2026-06-20 14:33:00', '2026-06-20 12:00:00'),
('5FEE4C52-8398-40C1-A550-7BDA70DEF6AC', 'D13A0975-0432-4FAA-86BB-8CE87DC80AD3', N'WeeklyProgressReport', N'تقرير جاهز للمراجعة', N'تم إنشاء تقرير إحالة جديد بانتظار مراجعتك واعتماده.', 1, '2026-06-22 13:26:00', '2026-06-22 12:00:00'),
('DB92FAC0-044F-47A4-9A2B-F8BD8209961B', 'D13A0975-0432-4FAA-86BB-8CE87DC80AD3', N'WeeklyProgressReport', N'تقرير جاهز للمراجعة', N'تم إنشاء تقرير إحالة جديد بانتظار مراجعتك واعتماده.', 1, '2026-06-29 15:17:00', '2026-06-29 12:00:00'),
('9A88685B-93DD-497D-BF27-A88442CE0E09', 'D13A0975-0432-4FAA-86BB-8CE87DC80AD3', N'AssessmentDue', N'تقييم بحاجة للمراجعة', N'أحد المرضى أكمل تقييماً نفسياً بحاجة لمراجعتك.', 0, NULL, '2026-07-02 12:00:00'),
('40C2A9D0-1581-485C-BB8D-3DC261E6F639', 'D13A0975-0432-4FAA-86BB-8CE87DC80AD3', N'WeeklyProgressReport', N'تقرير جاهز للمراجعة', N'تم إنشاء تقرير إحالة جديد بانتظار مراجعتك واعتماده.', 0, NULL, '2026-06-25 12:00:00'),
('69148DA4-04C2-43D4-B924-62249A52AE0A', 'D13A0975-0432-4FAA-86BB-8CE87DC80AD3', N'WeeklyProgressReport', N'تقرير جاهز للمراجعة', N'تم إنشاء تقرير إحالة جديد بانتظار مراجعتك واعتماده.', 0, NULL, '2026-06-19 12:00:00'),
('AA08F348-824A-4010-9FDD-0E957ED997FF', 'D13A0975-0432-4FAA-86BB-8CE87DC80AD3', N'WeeklyProgressReport', N'تقرير جاهز للمراجعة', N'تم إنشاء تقرير إحالة جديد بانتظار مراجعتك واعتماده.', 0, NULL, '2026-06-10 12:00:00'),
('CE218520-08FC-456B-B2AD-035ED638B40F', '645DD596-ACBA-4A99-B148-9E840EBDBC42', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة مجدولة مع أحد المرضى قريباً.', 1, '2026-06-23 18:30:00', '2026-06-23 12:00:00'),
('3A3C6FE6-EB2F-4199-ABD7-96DD5361D708', '645DD596-ACBA-4A99-B148-9E840EBDBC42', N'WeeklyProgressReport', N'تقرير جاهز للمراجعة', N'تم إنشاء تقرير إحالة جديد بانتظار مراجعتك واعتماده.', 1, '2026-06-21 19:20:00', '2026-06-21 12:00:00'),
('8734D0BC-1FD3-4920-B66C-49569CAA09C8', '645DD596-ACBA-4A99-B148-9E840EBDBC42', N'CrisisAlert', N'تنبيه أزمة', N'تم رصد مؤشر أزمة محتمل لدى أحد المرضى، يرجى المتابعة الفورية.', 1, '2026-06-24 14:50:00', '2026-06-24 12:00:00'),
('5818FB66-D0D1-46C2-8BA8-F4A1A2CBBFBE', '890607A3-3F87-400F-95A1-42171F2F95DD', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة مجدولة مع أحد المرضى قريباً.', 1, '2026-05-28 17:17:00', '2026-05-28 12:00:00'),
('0A9082DC-D6F5-4FB6-A09D-4AC7306FA71C', '890607A3-3F87-400F-95A1-42171F2F95DD', N'WeeklyProgressReport', N'تقرير جاهز للمراجعة', N'تم إنشاء تقرير إحالة جديد بانتظار مراجعتك واعتماده.', 0, NULL, '2026-06-18 12:00:00'),
('0602E21C-B3EC-419A-97BD-DC841514828B', '890607A3-3F87-400F-95A1-42171F2F95DD', N'WeeklyProgressReport', N'تقرير جاهز للمراجعة', N'تم إنشاء تقرير إحالة جديد بانتظار مراجعتك واعتماده.', 1, '2026-06-04 15:36:00', '2026-06-04 12:00:00'),
('BEA055CE-0CE0-43C5-B0E1-D0CBD644ECBA', '890607A3-3F87-400F-95A1-42171F2F95DD', N'WeeklyProgressReport', N'تقرير جاهز للمراجعة', N'تم إنشاء تقرير إحالة جديد بانتظار مراجعتك واعتماده.', 0, NULL, '2026-06-03 12:00:00'),
('723DAE57-9706-4C6E-9B67-45F8FCCA387D', '02A6C5EC-E76C-443D-AA05-C6C4FBEB839B', N'CrisisAlert', N'تنبيه أزمة', N'تم رصد مؤشر أزمة محتمل لدى أحد المرضى، يرجى المتابعة الفورية.', 1, '2026-06-10 12:22:00', '2026-06-10 12:00:00'),
('EF4C0D9E-88BB-4C01-BC85-5E468E110F84', '02A6C5EC-E76C-443D-AA05-C6C4FBEB839B', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة مجدولة مع أحد المرضى قريباً.', 1, '2026-06-30 12:48:00', '2026-06-30 12:00:00'),
('28A875E4-667C-4641-9633-C3D906C4AB41', '02A6C5EC-E76C-443D-AA05-C6C4FBEB839B', N'AssessmentDue', N'تقييم بحاجة للمراجعة', N'أحد المرضى أكمل تقييماً نفسياً بحاجة لمراجعتك.', 1, '2026-06-23 16:34:00', '2026-06-23 12:00:00'),
('20E65C14-CE32-4365-8E9E-0B4CC1EB066D', '02A6C5EC-E76C-443D-AA05-C6C4FBEB839B', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة مجدولة مع أحد المرضى قريباً.', 1, '2026-06-27 12:55:00', '2026-06-27 12:00:00'),
('4E7048FD-A20C-47A2-B34D-9027C8A65E0A', '0759431C-65BC-49A2-94FE-E906EBF11418', N'AssessmentDue', N'تقييم بحاجة للمراجعة', N'أحد المرضى أكمل تقييماً نفسياً بحاجة لمراجعتك.', 1, '2026-07-01 21:33:00', '2026-07-01 12:00:00'),
('EC87D2F3-14F1-4C80-A237-E629DCCE8920', '0759431C-65BC-49A2-94FE-E906EBF11418', N'WeeklyProgressReport', N'تقرير جاهز للمراجعة', N'تم إنشاء تقرير إحالة جديد بانتظار مراجعتك واعتماده.', 0, NULL, '2026-06-18 12:00:00'),
('3DBB6C06-80B3-4D27-8654-8E0D58DE1C05', '0759431C-65BC-49A2-94FE-E906EBF11418', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة مجدولة مع أحد المرضى قريباً.', 1, '2026-05-28 12:20:00', '2026-05-28 12:00:00'),
('A3E1A648-828D-49FB-916A-976A4E70748F', '0759431C-65BC-49A2-94FE-E906EBF11418', N'AssessmentDue', N'تقييم بحاجة للمراجعة', N'أحد المرضى أكمل تقييماً نفسياً بحاجة لمراجعتك.', 0, NULL, '2026-06-24 12:00:00'),
('07BD3C89-140D-4417-BFF2-158F2558466C', '0759431C-65BC-49A2-94FE-E906EBF11418', N'WeeklyProgressReport', N'تقرير جاهز للمراجعة', N'تم إنشاء تقرير إحالة جديد بانتظار مراجعتك واعتماده.', 1, '2026-06-15 17:22:00', '2026-06-15 12:00:00'),
('34E02A80-2899-493E-AE63-19594F04BB18', '0759431C-65BC-49A2-94FE-E906EBF11418', N'WeeklyProgressReport', N'تقرير جاهز للمراجعة', N'تم إنشاء تقرير إحالة جديد بانتظار مراجعتك واعتماده.', 1, '2026-07-02 19:42:00', '2026-07-02 12:00:00'),
('2D5C5079-D18B-4EEE-BBB9-FD78C7C3C0AB', '0759431C-65BC-49A2-94FE-E906EBF11418', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة مجدولة مع أحد المرضى قريباً.', 0, NULL, '2026-06-21 12:00:00'),
('B83B6746-D92A-4684-8FCF-56A512BF1C3D', '58D0A013-7F67-4666-AC48-44508CC29938', N'AssessmentDue', N'تقييم بحاجة للمراجعة', N'أحد المرضى أكمل تقييماً نفسياً بحاجة لمراجعتك.', 1, '2026-06-12 19:28:00', '2026-06-12 12:00:00'),
('8358E762-6F87-42ED-8753-56F0D391DB91', '58D0A013-7F67-4666-AC48-44508CC29938', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة مجدولة مع أحد المرضى قريباً.', 0, NULL, '2026-07-03 12:00:00'),
('8C52B2DE-D01C-4449-887F-112181D65A4C', '58D0A013-7F67-4666-AC48-44508CC29938', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة مجدولة مع أحد المرضى قريباً.', 1, '2026-06-12 17:53:00', '2026-06-12 12:00:00'),
('9AD59C5F-7ECE-42B5-ADF4-CE96C987A783', '58D0A013-7F67-4666-AC48-44508CC29938', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة مجدولة مع أحد المرضى قريباً.', 0, NULL, '2026-06-21 12:00:00'),
('783BC97B-409E-4F8B-B8E9-94DBEBD3D985', '58D0A013-7F67-4666-AC48-44508CC29938', N'AssessmentDue', N'تقييم بحاجة للمراجعة', N'أحد المرضى أكمل تقييماً نفسياً بحاجة لمراجعتك.', 0, NULL, '2026-06-08 12:00:00'),
('AB3A012F-3857-4CF1-B8C0-A04C2293CB96', '8D78ED99-7BD8-4ED9-B888-243669B55907', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة مجدولة مع أحد المرضى قريباً.', 1, '2026-05-25 12:25:00', '2026-05-25 12:00:00'),
('57F8CDB0-DCCA-436D-8E35-7F9798376D3A', '8D78ED99-7BD8-4ED9-B888-243669B55907', N'AssessmentDue', N'تقييم بحاجة للمراجعة', N'أحد المرضى أكمل تقييماً نفسياً بحاجة لمراجعتك.', 1, '2026-06-21 12:50:00', '2026-06-21 12:00:00'),
('7FCA221F-F358-480C-9DED-CEF7550CD25A', '8D78ED99-7BD8-4ED9-B888-243669B55907', N'AssessmentDue', N'تقييم بحاجة للمراجعة', N'أحد المرضى أكمل تقييماً نفسياً بحاجة لمراجعتك.', 1, '2026-06-18 17:23:00', '2026-06-18 12:00:00'),
('A6AC19F5-A5A1-4BF4-94DF-D964E5196A82', '8D78ED99-7BD8-4ED9-B888-243669B55907', N'CrisisAlert', N'تنبيه أزمة', N'تم رصد مؤشر أزمة محتمل لدى أحد المرضى، يرجى المتابعة الفورية.', 1, '2026-06-21 15:54:00', '2026-06-21 12:00:00'),
('192D54F1-0BE6-4D8C-94EC-88F7F60DC8F3', '8D78ED99-7BD8-4ED9-B888-243669B55907', N'CrisisAlert', N'تنبيه أزمة', N'تم رصد مؤشر أزمة محتمل لدى أحد المرضى، يرجى المتابعة الفورية.', 1, '2026-06-16 21:49:00', '2026-06-16 12:00:00'),
('597D105C-118F-431D-B566-C4445732BB39', '8D78ED99-7BD8-4ED9-B888-243669B55907', N'AssessmentDue', N'تقييم بحاجة للمراجعة', N'أحد المرضى أكمل تقييماً نفسياً بحاجة لمراجعتك.', 0, NULL, '2026-06-16 12:00:00'),
('950CB407-1F8A-475E-A0A2-F61F2B6DED9D', '8D78ED99-7BD8-4ED9-B888-243669B55907', N'WeeklyProgressReport', N'تقرير جاهز للمراجعة', N'تم إنشاء تقرير إحالة جديد بانتظار مراجعتك واعتماده.', 1, '2026-06-12 21:40:00', '2026-06-12 12:00:00'),
('E8E251F7-06E5-485B-A788-C64A37CF57FE', '8D78ED99-7BD8-4ED9-B888-243669B55907', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة مجدولة مع أحد المرضى قريباً.', 1, '2026-05-27 20:27:00', '2026-05-27 12:00:00'),
('EE2927AD-5744-4441-BA18-9CACDF6F4D11', 'B06C2022-7181-4B83-9000-D2D7BCAA14F8', N'AssessmentDue', N'تقييم جديد متاح', N'تم إتاحة تقييم نفسي جديد لمراجعته وتعبئته.', 0, NULL, '2026-06-28 12:00:00'),
('3EA1FDEF-00E4-421E-A6DA-4EE8688F4B77', 'B06C2022-7181-4B83-9000-D2D7BCAA14F8', N'WeeklyProgressReport', N'تقرير التقدم الأسبوعي', N'تقرير تقدمك الأسبوعي جاهز الآن للمراجعة.', 0, NULL, '2026-06-21 12:00:00'),
('CB8D4496-1FFD-4806-966F-B2DF17E60863', 'B06C2022-7181-4B83-9000-D2D7BCAA14F8', N'SessionCancelled', N'تم إلغاء الجلسة', N'تم إلغاء الجلسة المقررة، سيتم التواصل معك لتحديد موعد جديد.', 1, '2026-05-24 15:52:00', '2026-05-24 12:00:00'),
('A16A4036-DF74-482F-907B-C49A36A8F72F', 'B06C2022-7181-4B83-9000-D2D7BCAA14F8', N'ExerciseReminder', N'تذكير بتمرين', N'حان وقت أداء التمرين المخصص لك اليوم.', 1, '2026-07-02 17:46:00', '2026-07-02 12:00:00'),
('623D11C0-7F63-46A5-8E33-F3A61C782FD2', 'B06C2022-7181-4B83-9000-D2D7BCAA14F8', N'ExerciseReminder', N'تذكير بتمرين', N'حان وقت أداء التمرين المخصص لك اليوم.', 1, '2026-05-25 13:49:00', '2026-05-25 12:00:00'),
('C86BF570-6410-4652-84E1-9730E6FCD1D9', 'F9C10CB6-CD45-47B2-9BB6-91CC985F8DC5', N'SessionCancelled', N'تم إلغاء الجلسة', N'تم إلغاء الجلسة المقررة، سيتم التواصل معك لتحديد موعد جديد.', 1, '2026-06-18 12:29:00', '2026-06-18 12:00:00'),
('1712340D-92F3-4B53-83BA-553C4EA7FEE8', 'F9C10CB6-CD45-47B2-9BB6-91CC985F8DC5', N'AssessmentDue', N'تقييم جديد متاح', N'تم إتاحة تقييم نفسي جديد لمراجعته وتعبئته.', 1, '2026-07-02 14:09:00', '2026-07-02 12:00:00'),
('FAA68422-35BB-41FF-9499-3CF2B4A5206B', 'F9C10CB6-CD45-47B2-9BB6-91CC985F8DC5', N'WeeklyProgressReport', N'تقرير التقدم الأسبوعي', N'تقرير تقدمك الأسبوعي جاهز الآن للمراجعة.', 0, NULL, '2026-06-04 12:00:00'),
('D53560C1-2797-46EF-A888-5E54E6650BD2', 'F9C10CB6-CD45-47B2-9BB6-91CC985F8DC5', N'WeeklyProgressReport', N'تقرير التقدم الأسبوعي', N'تقرير تقدمك الأسبوعي جاهز الآن للمراجعة.', 0, NULL, '2026-07-05 12:00:00'),
('C141AD08-E961-4A62-BBEB-6D2929B37945', 'F9C10CB6-CD45-47B2-9BB6-91CC985F8DC5', N'AssessmentDue', N'تقييم جديد متاح', N'تم إتاحة تقييم نفسي جديد لمراجعته وتعبئته.', 1, '2026-05-24 17:13:00', '2026-05-24 12:00:00'),
('51517838-E315-41C4-BFCE-20B73FD35A4F', 'F9C10CB6-CD45-47B2-9BB6-91CC985F8DC5', N'ExerciseReminder', N'تذكير بتمرين', N'حان وقت أداء التمرين المخصص لك اليوم.', 1, '2026-06-11 21:55:00', '2026-06-11 12:00:00'),
('A6BFB88A-5254-420B-9C36-28791F715647', '262F5E5C-0885-4572-BD7A-247EFD15D376', N'ExerciseReminder', N'تذكير بتمرين', N'حان وقت أداء التمرين المخصص لك اليوم.', 1, '2026-06-20 12:22:00', '2026-06-20 12:00:00'),
('360CDD3D-5E39-4CBF-A1B5-C0B7D689FD64', '262F5E5C-0885-4572-BD7A-247EFD15D376', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة علاجية مجدولة قريباً، يرجى الحضور في الموعد.', 1, '2026-07-03 17:37:00', '2026-07-03 12:00:00'),
('1CF2B6FB-6F0E-4B1F-98E9-907AED5012CB', '262F5E5C-0885-4572-BD7A-247EFD15D376', N'SessionCancelled', N'تم إلغاء الجلسة', N'تم إلغاء الجلسة المقررة، سيتم التواصل معك لتحديد موعد جديد.', 1, '2026-05-24 16:28:00', '2026-05-24 12:00:00'),
('47E9098C-2A26-43C9-8AB6-C3E9E0BA2670', '262F5E5C-0885-4572-BD7A-247EFD15D376', N'WeeklyProgressReport', N'تقرير التقدم الأسبوعي', N'تقرير تقدمك الأسبوعي جاهز الآن للمراجعة.', 0, NULL, '2026-06-20 12:00:00'),
('8443CF59-0211-46FF-A53A-2A434AEE39D9', '262F5E5C-0885-4572-BD7A-247EFD15D376', N'WeeklyProgressReport', N'تقرير التقدم الأسبوعي', N'تقرير تقدمك الأسبوعي جاهز الآن للمراجعة.', 1, '2026-06-12 18:57:00', '2026-06-12 12:00:00'),
('E61C0FB0-EBFB-4F84-BC21-6B84EDD77AC2', '262F5E5C-0885-4572-BD7A-247EFD15D376', N'ExerciseReminder', N'تذكير بتمرين', N'حان وقت أداء التمرين المخصص لك اليوم.', 0, NULL, '2026-05-31 12:00:00'),
('6EBDEB51-BD68-42BC-9CA5-352FA1F3C4AB', '262F5E5C-0885-4572-BD7A-247EFD15D376', N'ExerciseReminder', N'تذكير بتمرين', N'حان وقت أداء التمرين المخصص لك اليوم.', 1, '2026-05-25 14:21:00', '2026-05-25 12:00:00'),
('8ECE2FF8-9A2A-4B04-A386-ED6F583FD5E3', '8E6E5EAC-2176-4F0B-9C3D-BADAEF969807', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة علاجية مجدولة قريباً، يرجى الحضور في الموعد.', 1, '2026-06-19 15:34:00', '2026-06-19 12:00:00'),
('1D963152-0241-4CC2-8E90-A8D40B700E7A', '8E6E5EAC-2176-4F0B-9C3D-BADAEF969807', N'AssessmentDue', N'تقييم جديد متاح', N'تم إتاحة تقييم نفسي جديد لمراجعته وتعبئته.', 1, '2026-06-30 18:45:00', '2026-06-30 12:00:00'),
('372D1DEA-FDDC-4D90-97DF-71203E4639F3', '8E6E5EAC-2176-4F0B-9C3D-BADAEF969807', N'AssessmentDue', N'تقييم جديد متاح', N'تم إتاحة تقييم نفسي جديد لمراجعته وتعبئته.', 0, NULL, '2026-06-08 12:00:00'),
('EEC2C7BE-5E8C-429A-9620-C4200D1674C7', 'E0FFDBC2-F09B-4996-88E4-9F4B23D6F0FF', N'ExerciseReminder', N'تذكير بتمرين', N'حان وقت أداء التمرين المخصص لك اليوم.', 1, '2026-05-22 21:08:00', '2026-05-22 12:00:00'),
('8F273443-9D51-4148-AE47-5536FF2B9FD5', 'E0FFDBC2-F09B-4996-88E4-9F4B23D6F0FF', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة علاجية مجدولة قريباً، يرجى الحضور في الموعد.', 0, NULL, '2026-05-31 12:00:00'),
('95D4E084-D8D1-4BBB-9A18-3C2596A8F643', 'E0FFDBC2-F09B-4996-88E4-9F4B23D6F0FF', N'ExerciseReminder', N'تذكير بتمرين', N'حان وقت أداء التمرين المخصص لك اليوم.', 0, NULL, '2026-06-22 12:00:00'),
('4FD9665E-4763-4946-8FB1-E7F3FE562CA1', 'E0FFDBC2-F09B-4996-88E4-9F4B23D6F0FF', N'WeeklyProgressReport', N'تقرير التقدم الأسبوعي', N'تقرير تقدمك الأسبوعي جاهز الآن للمراجعة.', 0, NULL, '2026-07-06 12:00:00'),
('DD02971B-9135-4537-923E-5D105ADA3A22', '72F60FBA-CE9B-41DC-B960-1EAC6D43143D', N'ExerciseReminder', N'تذكير بتمرين', N'حان وقت أداء التمرين المخصص لك اليوم.', 0, NULL, '2026-06-24 12:00:00'),
('6BE85E02-CCD8-4456-B123-9325508EA57E', '72F60FBA-CE9B-41DC-B960-1EAC6D43143D', N'WeeklyProgressReport', N'تقرير التقدم الأسبوعي', N'تقرير تقدمك الأسبوعي جاهز الآن للمراجعة.', 1, '2026-06-23 17:24:00', '2026-06-23 12:00:00'),
('2E73D892-4070-4A55-A931-5EA3A5A2D2B8', '72F60FBA-CE9B-41DC-B960-1EAC6D43143D', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة علاجية مجدولة قريباً، يرجى الحضور في الموعد.', 1, '2026-06-21 12:23:00', '2026-06-21 12:00:00'),
('6CD6766B-B124-4467-9C83-120F8B59B7B8', '72F60FBA-CE9B-41DC-B960-1EAC6D43143D', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة علاجية مجدولة قريباً، يرجى الحضور في الموعد.', 0, NULL, '2026-05-31 12:00:00'),
('8512FB31-B4B8-4A7E-A6A1-9C4567793D47', '72F60FBA-CE9B-41DC-B960-1EAC6D43143D', N'WeeklyProgressReport', N'تقرير التقدم الأسبوعي', N'تقرير تقدمك الأسبوعي جاهز الآن للمراجعة.', 1, '2026-06-10 17:01:00', '2026-06-10 12:00:00'),
('570C2DFC-F6FB-47D8-83B5-4A9EA531F95A', '72F60FBA-CE9B-41DC-B960-1EAC6D43143D', N'SessionCancelled', N'تم إلغاء الجلسة', N'تم إلغاء الجلسة المقررة، سيتم التواصل معك لتحديد موعد جديد.', 1, '2026-06-14 14:49:00', '2026-06-14 12:00:00'),
('52D7181F-62F3-4998-927B-82D82AAC2ADA', '8F464A15-DAAB-4952-BF75-064FCFD765E7', N'ExerciseReminder', N'تذكير بتمرين', N'حان وقت أداء التمرين المخصص لك اليوم.', 1, '2026-06-29 17:55:00', '2026-06-29 12:00:00'),
('B8507A6B-9E37-4DCA-84AA-07C8913C3532', '8F464A15-DAAB-4952-BF75-064FCFD765E7', N'SessionCancelled', N'تم إلغاء الجلسة', N'تم إلغاء الجلسة المقررة، سيتم التواصل معك لتحديد موعد جديد.', 0, NULL, '2026-07-01 12:00:00'),
('14C353FC-4C0C-4D4E-9127-27F24CAACBDC', '8F464A15-DAAB-4952-BF75-064FCFD765E7', N'WeeklyProgressReport', N'تقرير التقدم الأسبوعي', N'تقرير تقدمك الأسبوعي جاهز الآن للمراجعة.', 0, NULL, '2026-06-23 12:00:00'),
('C5965FC7-1406-4ED1-AC63-17D2A787ED79', '8F464A15-DAAB-4952-BF75-064FCFD765E7', N'AssessmentDue', N'تقييم جديد متاح', N'تم إتاحة تقييم نفسي جديد لمراجعته وتعبئته.', 1, '2026-07-02 21:37:00', '2026-07-02 12:00:00'),
('1108E26D-236C-4ED9-9F6D-CE950CB087FD', '8F464A15-DAAB-4952-BF75-064FCFD765E7', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة علاجية مجدولة قريباً، يرجى الحضور في الموعد.', 0, NULL, '2026-05-26 12:00:00'),
('D923A315-A4C1-484E-863B-02389F1F8615', '8F464A15-DAAB-4952-BF75-064FCFD765E7', N'WeeklyProgressReport', N'تقرير التقدم الأسبوعي', N'تقرير تقدمك الأسبوعي جاهز الآن للمراجعة.', 0, NULL, '2026-05-30 12:00:00'),
('6AA6EBBB-E188-4DD1-9EF3-5B2129C5A998', '8F464A15-DAAB-4952-BF75-064FCFD765E7', N'WeeklyProgressReport', N'تقرير التقدم الأسبوعي', N'تقرير تقدمك الأسبوعي جاهز الآن للمراجعة.', 0, NULL, '2026-05-26 12:00:00'),
('5867F62D-D620-4D69-8582-B5BE7C856996', '8F464A15-DAAB-4952-BF75-064FCFD765E7', N'AssessmentDue', N'تقييم جديد متاح', N'تم إتاحة تقييم نفسي جديد لمراجعته وتعبئته.', 1, '2026-06-11 18:29:00', '2026-06-11 12:00:00'),
('7F000BEA-DDE4-472D-B828-660415FC2D2B', 'A4D9297C-F020-4C75-8F70-207DD99016FE', N'ExerciseReminder', N'تذكير بتمرين', N'حان وقت أداء التمرين المخصص لك اليوم.', 1, '2026-06-09 19:10:00', '2026-06-09 12:00:00'),
('F8E20A6A-68E2-4CAA-B377-EC3E22926307', 'A4D9297C-F020-4C75-8F70-207DD99016FE', N'SessionCancelled', N'تم إلغاء الجلسة', N'تم إلغاء الجلسة المقررة، سيتم التواصل معك لتحديد موعد جديد.', 1, '2026-06-28 20:12:00', '2026-06-28 12:00:00'),
('7F22BA63-4D29-4F11-90C9-30AFE8AE9668', 'A4D9297C-F020-4C75-8F70-207DD99016FE', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة علاجية مجدولة قريباً، يرجى الحضور في الموعد.', 0, NULL, '2026-06-28 12:00:00'),
('770C4E31-B216-45DB-A2C4-AB4EEF08CD12', 'A4D9297C-F020-4C75-8F70-207DD99016FE', N'WeeklyProgressReport', N'تقرير التقدم الأسبوعي', N'تقرير تقدمك الأسبوعي جاهز الآن للمراجعة.', 0, NULL, '2026-06-20 12:00:00'),
('F549B141-2604-4926-B5C1-7BAD048D4EAB', 'A4D9297C-F020-4C75-8F70-207DD99016FE', N'AssessmentDue', N'تقييم جديد متاح', N'تم إتاحة تقييم نفسي جديد لمراجعته وتعبئته.', 0, NULL, '2026-07-03 12:00:00'),
('E93F1C74-E674-463B-9C06-A35A66AD0999', 'A4D9297C-F020-4C75-8F70-207DD99016FE', N'WeeklyProgressReport', N'تقرير التقدم الأسبوعي', N'تقرير تقدمك الأسبوعي جاهز الآن للمراجعة.', 1, '2026-06-02 12:34:00', '2026-06-02 12:00:00'),
('F38D44C1-4F46-424E-AA39-5B3E02A8D0AE', 'A4D9297C-F020-4C75-8F70-207DD99016FE', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة علاجية مجدولة قريباً، يرجى الحضور في الموعد.', 0, NULL, '2026-06-05 12:00:00'),
('CDA05C73-5D77-477B-B21B-EA21B99D301D', 'DB091DDD-6138-42BD-B1BD-F8D5DCF6DF96', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة علاجية مجدولة قريباً، يرجى الحضور في الموعد.', 1, '2026-06-24 20:40:00', '2026-06-24 12:00:00'),
('7E36A2D8-B9C8-409E-B1E3-5862369EE6AB', 'DB091DDD-6138-42BD-B1BD-F8D5DCF6DF96', N'ExerciseReminder', N'تذكير بتمرين', N'حان وقت أداء التمرين المخصص لك اليوم.', 1, '2026-06-19 17:37:00', '2026-06-19 12:00:00'),
('E7A558AD-E1FC-4277-889A-5506BE1C3B57', 'DB091DDD-6138-42BD-B1BD-F8D5DCF6DF96', N'WeeklyProgressReport', N'تقرير التقدم الأسبوعي', N'تقرير تقدمك الأسبوعي جاهز الآن للمراجعة.', 1, '2026-06-19 14:44:00', '2026-06-19 12:00:00'),
('FA5F0206-B3E3-4582-B9DA-C9A4A49B284F', 'DB091DDD-6138-42BD-B1BD-F8D5DCF6DF96', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة علاجية مجدولة قريباً، يرجى الحضور في الموعد.', 1, '2026-05-29 15:24:00', '2026-05-29 12:00:00'),
('D736145C-C92D-4F7A-AB41-4886CC2D58F3', 'DB091DDD-6138-42BD-B1BD-F8D5DCF6DF96', N'AssessmentDue', N'تقييم جديد متاح', N'تم إتاحة تقييم نفسي جديد لمراجعته وتعبئته.', 1, '2026-06-15 12:09:00', '2026-06-15 12:00:00'),
('7758363A-359F-45E0-B0C6-BB39D89C5694', 'DB091DDD-6138-42BD-B1BD-F8D5DCF6DF96', N'ExerciseReminder', N'تذكير بتمرين', N'حان وقت أداء التمرين المخصص لك اليوم.', 0, NULL, '2026-05-22 12:00:00'),
('26979094-011A-4967-9B5A-C9115C2C036F', 'DB091DDD-6138-42BD-B1BD-F8D5DCF6DF96', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة علاجية مجدولة قريباً، يرجى الحضور في الموعد.', 0, NULL, '2026-07-03 12:00:00'),
('6A69F1F3-68F5-42D3-87FE-7E5415AF2927', '65558633-FEE9-48B7-8AF8-40BFD220B851', N'ExerciseReminder', N'تذكير بتمرين', N'حان وقت أداء التمرين المخصص لك اليوم.', 1, '2026-06-13 19:28:00', '2026-06-13 12:00:00'),
('A11E3CB6-53F1-4AEC-854C-B1BA6D6D5001', '65558633-FEE9-48B7-8AF8-40BFD220B851', N'AssessmentDue', N'تقييم جديد متاح', N'تم إتاحة تقييم نفسي جديد لمراجعته وتعبئته.', 1, '2026-06-22 18:19:00', '2026-06-22 12:00:00'),
('7EBD1106-E646-4216-B380-26EA8E7D869D', '65558633-FEE9-48B7-8AF8-40BFD220B851', N'ExerciseReminder', N'تذكير بتمرين', N'حان وقت أداء التمرين المخصص لك اليوم.', 1, '2026-06-28 18:39:00', '2026-06-28 12:00:00'),
('EA2F3077-9236-46DD-9AAB-84CEA80FEF89', '65558633-FEE9-48B7-8AF8-40BFD220B851', N'ExerciseReminder', N'تذكير بتمرين', N'حان وقت أداء التمرين المخصص لك اليوم.', 0, NULL, '2026-06-09 12:00:00'),
('C7BDFA5D-74FC-49EF-8E82-94CF494F9DF2', '65558633-FEE9-48B7-8AF8-40BFD220B851', N'AssessmentDue', N'تقييم جديد متاح', N'تم إتاحة تقييم نفسي جديد لمراجعته وتعبئته.', 0, NULL, '2026-05-25 12:00:00'),
('4391DCB8-6AAD-44AE-9829-962DCAA7EBC6', '82C584D7-9870-4182-8752-2AB1097712D3', N'WeeklyProgressReport', N'تقرير التقدم الأسبوعي', N'تقرير تقدمك الأسبوعي جاهز الآن للمراجعة.', 0, NULL, '2026-07-02 12:00:00'),
('84A5CDDF-FB8F-486D-B3A7-1476009CCA87', '82C584D7-9870-4182-8752-2AB1097712D3', N'ExerciseReminder', N'تذكير بتمرين', N'حان وقت أداء التمرين المخصص لك اليوم.', 1, '2026-06-01 15:36:00', '2026-06-01 12:00:00'),
('0DB2DE92-B343-4094-9C73-29FD665D742E', '82C584D7-9870-4182-8752-2AB1097712D3', N'ExerciseReminder', N'تذكير بتمرين', N'حان وقت أداء التمرين المخصص لك اليوم.', 1, '2026-06-12 17:23:00', '2026-06-12 12:00:00'),
('76F60A34-1A5C-42D2-A182-80C155A593B6', 'AECAB352-8D01-4258-9451-A98DA466761F', N'SessionCancelled', N'تم إلغاء الجلسة', N'تم إلغاء الجلسة المقررة، سيتم التواصل معك لتحديد موعد جديد.', 0, NULL, '2026-06-14 12:00:00'),
('80CEFC13-31E9-4808-A4EE-55A3A78E7120', 'AECAB352-8D01-4258-9451-A98DA466761F', N'ExerciseReminder', N'تذكير بتمرين', N'حان وقت أداء التمرين المخصص لك اليوم.', 1, '2026-06-01 18:28:00', '2026-06-01 12:00:00'),
('C7BBF4C7-991A-4E74-9DC4-722F36BDC38F', 'AECAB352-8D01-4258-9451-A98DA466761F', N'AssessmentDue', N'تقييم جديد متاح', N'تم إتاحة تقييم نفسي جديد لمراجعته وتعبئته.', 0, NULL, '2026-07-05 12:00:00'),
('CF8EAA39-4749-42B1-8377-AFA6DE2B1D1C', 'AECAB352-8D01-4258-9451-A98DA466761F', N'WeeklyProgressReport', N'تقرير التقدم الأسبوعي', N'تقرير تقدمك الأسبوعي جاهز الآن للمراجعة.', 1, '2026-05-29 19:32:00', '2026-05-29 12:00:00'),
('E660208B-B730-4E19-876E-B8156AD10860', 'AECAB352-8D01-4258-9451-A98DA466761F', N'SessionCancelled', N'تم إلغاء الجلسة', N'تم إلغاء الجلسة المقررة، سيتم التواصل معك لتحديد موعد جديد.', 1, '2026-06-09 17:11:00', '2026-06-09 12:00:00'),
('F8262439-5A8F-4A47-87CF-8970DEE156F7', 'ED2A4F7A-6392-466E-9979-51CD2AC1FE2E', N'WeeklyProgressReport', N'تقرير التقدم الأسبوعي', N'تقرير تقدمك الأسبوعي جاهز الآن للمراجعة.', 1, '2026-07-03 18:29:00', '2026-07-03 12:00:00'),
('168106DA-9B0E-4DD7-AA9C-710B0855A7A1', 'ED2A4F7A-6392-466E-9979-51CD2AC1FE2E', N'WeeklyProgressReport', N'تقرير التقدم الأسبوعي', N'تقرير تقدمك الأسبوعي جاهز الآن للمراجعة.', 0, NULL, '2026-06-18 12:00:00'),
('35F8D23B-C4F9-49B6-840D-FF2C357E1474', 'ED2A4F7A-6392-466E-9979-51CD2AC1FE2E', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة علاجية مجدولة قريباً، يرجى الحضور في الموعد.', 1, '2026-06-30 13:42:00', '2026-06-30 12:00:00'),
('C2A60A9B-6EF9-4B85-A4AE-32FFFA6DF46E', 'ED2A4F7A-6392-466E-9979-51CD2AC1FE2E', N'AssessmentDue', N'تقييم جديد متاح', N'تم إتاحة تقييم نفسي جديد لمراجعته وتعبئته.', 1, '2026-06-21 20:17:00', '2026-06-21 12:00:00'),
('3A694115-8094-4DBA-A1B9-DBFC9421BA40', 'ED2A4F7A-6392-466E-9979-51CD2AC1FE2E', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة علاجية مجدولة قريباً، يرجى الحضور في الموعد.', 1, '2026-06-06 18:50:00', '2026-06-06 12:00:00'),
('8A95AEE2-1580-4C6B-AA91-6410336AB097', 'ED2A4F7A-6392-466E-9979-51CD2AC1FE2E', N'WeeklyProgressReport', N'تقرير التقدم الأسبوعي', N'تقرير تقدمك الأسبوعي جاهز الآن للمراجعة.', 0, NULL, '2026-06-22 12:00:00'),
('2BA39AC5-205E-42EA-9D8E-D681676AC225', 'ED2A4F7A-6392-466E-9979-51CD2AC1FE2E', N'ExerciseReminder', N'تذكير بتمرين', N'حان وقت أداء التمرين المخصص لك اليوم.', 1, '2026-06-26 16:01:00', '2026-06-26 12:00:00'),
('5363B63C-7D53-4DA5-8F91-BED44DDEB4CC', 'ED2A4F7A-6392-466E-9979-51CD2AC1FE2E', N'WeeklyProgressReport', N'تقرير التقدم الأسبوعي', N'تقرير تقدمك الأسبوعي جاهز الآن للمراجعة.', 0, NULL, '2026-06-16 12:00:00'),
('73F6B508-A533-446D-90FF-E26175481CF9', 'A45399A0-E659-442B-8C4A-F0378C9E2347', N'WeeklyProgressReport', N'تقرير التقدم الأسبوعي', N'تقرير تقدمك الأسبوعي جاهز الآن للمراجعة.', 1, '2026-06-19 20:43:00', '2026-06-19 12:00:00'),
('98D5B977-3376-4CC7-9319-0FBC88774695', 'A45399A0-E659-442B-8C4A-F0378C9E2347', N'ExerciseReminder', N'تذكير بتمرين', N'حان وقت أداء التمرين المخصص لك اليوم.', 0, NULL, '2026-06-22 12:00:00'),
('F8D17631-9503-4850-8DE7-12C8F37A62FC', 'A45399A0-E659-442B-8C4A-F0378C9E2347', N'WeeklyProgressReport', N'تقرير التقدم الأسبوعي', N'تقرير تقدمك الأسبوعي جاهز الآن للمراجعة.', 0, NULL, '2026-06-30 12:00:00'),
('71C53047-1F21-4A08-8EC3-DB903EE25796', '3E18E043-8106-4F93-A0C5-8BFECBF20132', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة علاجية مجدولة قريباً، يرجى الحضور في الموعد.', 1, '2026-06-16 19:10:00', '2026-06-16 12:00:00'),
('E0BBE14F-8FD7-41DC-95F2-DF5A22F8C2F2', '3E18E043-8106-4F93-A0C5-8BFECBF20132', N'SessionCancelled', N'تم إلغاء الجلسة', N'تم إلغاء الجلسة المقررة، سيتم التواصل معك لتحديد موعد جديد.', 1, '2026-06-14 12:58:00', '2026-06-14 12:00:00'),
('5F17A11A-3207-40B2-A2B7-323A96BDC5C7', '3E18E043-8106-4F93-A0C5-8BFECBF20132', N'SessionCancelled', N'تم إلغاء الجلسة', N'تم إلغاء الجلسة المقررة، سيتم التواصل معك لتحديد موعد جديد.', 0, NULL, '2026-06-29 12:00:00'),
('AA51FFB0-6B56-41D5-81C4-FE1F5DC34A54', '3E18E043-8106-4F93-A0C5-8BFECBF20132', N'SessionCancelled', N'تم إلغاء الجلسة', N'تم إلغاء الجلسة المقررة، سيتم التواصل معك لتحديد موعد جديد.', 1, '2026-06-27 17:13:00', '2026-06-27 12:00:00'),
('214FEE38-1579-4045-82B8-A5CB66DE8169', 'A7CCD1A6-1AEF-426F-A1D4-DAAA45EEFCD8', N'AssessmentDue', N'تقييم جديد متاح', N'تم إتاحة تقييم نفسي جديد لمراجعته وتعبئته.', 1, '2026-06-01 19:45:00', '2026-06-01 12:00:00'),
('5E643534-FEDD-4268-B25C-F36DF11112AC', 'A7CCD1A6-1AEF-426F-A1D4-DAAA45EEFCD8', N'ExerciseReminder', N'تذكير بتمرين', N'حان وقت أداء التمرين المخصص لك اليوم.', 1, '2026-06-30 15:43:00', '2026-06-30 12:00:00'),
('E673E73C-9CAB-4370-95E5-72227879FA57', 'A7CCD1A6-1AEF-426F-A1D4-DAAA45EEFCD8', N'WeeklyProgressReport', N'تقرير التقدم الأسبوعي', N'تقرير تقدمك الأسبوعي جاهز الآن للمراجعة.', 1, '2026-06-03 12:42:00', '2026-06-03 12:00:00'),
('E9CF08F9-8E04-45F0-B777-766539245F76', 'A7CCD1A6-1AEF-426F-A1D4-DAAA45EEFCD8', N'WeeklyProgressReport', N'تقرير التقدم الأسبوعي', N'تقرير تقدمك الأسبوعي جاهز الآن للمراجعة.', 0, NULL, '2026-06-14 12:00:00'),
('FBE9DF67-D54C-4E77-BB76-689A876BAD87', 'A7CCD1A6-1AEF-426F-A1D4-DAAA45EEFCD8', N'AssessmentDue', N'تقييم جديد متاح', N'تم إتاحة تقييم نفسي جديد لمراجعته وتعبئته.', 1, '2026-07-02 20:37:00', '2026-07-02 12:00:00'),
('128062FB-0C82-4539-B9AD-5DB6B4494DAE', '250B7FE2-5C94-432C-89B9-80AF87185FC0', N'AssessmentDue', N'تقييم جديد متاح', N'تم إتاحة تقييم نفسي جديد لمراجعته وتعبئته.', 0, NULL, '2026-07-03 12:00:00'),
('FBF0EEE4-07CC-41AC-9C65-6556B6DA620C', '250B7FE2-5C94-432C-89B9-80AF87185FC0', N'ExerciseReminder', N'تذكير بتمرين', N'حان وقت أداء التمرين المخصص لك اليوم.', 0, NULL, '2026-06-10 12:00:00'),
('87FC68DD-585E-4764-8F8B-C92BDA416FE4', '250B7FE2-5C94-432C-89B9-80AF87185FC0', N'ExerciseReminder', N'تذكير بتمرين', N'حان وقت أداء التمرين المخصص لك اليوم.', 1, '2026-06-17 20:52:00', '2026-06-17 12:00:00'),
('36513BF1-D0E1-4D46-954B-3EA1555A592A', '250B7FE2-5C94-432C-89B9-80AF87185FC0', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة علاجية مجدولة قريباً، يرجى الحضور في الموعد.', 1, '2026-07-05 13:01:00', '2026-07-05 12:00:00'),
('6E97FDEB-3C07-4046-85C2-E135F4646067', '40AF507D-2E94-40AA-9268-B0BBA290493B', N'WeeklyProgressReport', N'تقرير التقدم الأسبوعي', N'تقرير تقدمك الأسبوعي جاهز الآن للمراجعة.', 1, '2026-06-09 15:12:00', '2026-06-09 12:00:00'),
('A898CBC8-E169-40FC-B2D5-47AB74CF200C', '40AF507D-2E94-40AA-9268-B0BBA290493B', N'SessionCancelled', N'تم إلغاء الجلسة', N'تم إلغاء الجلسة المقررة، سيتم التواصل معك لتحديد موعد جديد.', 0, NULL, '2026-06-26 12:00:00'),
('06CE4C14-BDFB-40FF-A54F-05BEB605122B', '40AF507D-2E94-40AA-9268-B0BBA290493B', N'AssessmentDue', N'تقييم جديد متاح', N'تم إتاحة تقييم نفسي جديد لمراجعته وتعبئته.', 0, NULL, '2026-05-29 12:00:00'),
('73D01064-0E1D-4AFD-AEA9-B891F1E5284E', '40AF507D-2E94-40AA-9268-B0BBA290493B', N'SessionCancelled', N'تم إلغاء الجلسة', N'تم إلغاء الجلسة المقررة، سيتم التواصل معك لتحديد موعد جديد.', 1, '2026-05-25 20:45:00', '2026-05-25 12:00:00'),
('D5837D32-A2A6-4A71-911E-EBE92D948C80', '40AF507D-2E94-40AA-9268-B0BBA290493B', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة علاجية مجدولة قريباً، يرجى الحضور في الموعد.', 0, NULL, '2026-06-24 12:00:00'),
('26BD7F34-FAC9-4249-83D2-31B98535AABF', '40AF507D-2E94-40AA-9268-B0BBA290493B', N'SessionCancelled', N'تم إلغاء الجلسة', N'تم إلغاء الجلسة المقررة، سيتم التواصل معك لتحديد موعد جديد.', 1, '2026-06-22 16:42:00', '2026-06-22 12:00:00'),
('6EBFA213-ED5A-4E80-8B9B-75DBFECA3139', '40AF507D-2E94-40AA-9268-B0BBA290493B', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة علاجية مجدولة قريباً، يرجى الحضور في الموعد.', 0, NULL, '2026-07-01 12:00:00'),
('E774329A-FB5A-4AFC-9AE0-EA436568792B', 'E64A2B73-C807-421A-BBDE-A9FCA7D29587', N'ExerciseReminder', N'تذكير بتمرين', N'حان وقت أداء التمرين المخصص لك اليوم.', 1, '2026-05-31 13:28:00', '2026-05-31 12:00:00'),
('50D61467-0E4B-4CEC-A2D7-56B63421F04B', 'E64A2B73-C807-421A-BBDE-A9FCA7D29587', N'WeeklyProgressReport', N'تقرير التقدم الأسبوعي', N'تقرير تقدمك الأسبوعي جاهز الآن للمراجعة.', 0, NULL, '2026-06-24 12:00:00'),
('D59FF5F4-4B01-4566-BD12-DB4462D6611D', 'E64A2B73-C807-421A-BBDE-A9FCA7D29587', N'WeeklyProgressReport', N'تقرير التقدم الأسبوعي', N'تقرير تقدمك الأسبوعي جاهز الآن للمراجعة.', 0, NULL, '2026-07-02 12:00:00'),
('A7367D1D-D7F3-42C6-8DE9-1D9D6118F994', 'E64A2B73-C807-421A-BBDE-A9FCA7D29587', N'SessionCancelled', N'تم إلغاء الجلسة', N'تم إلغاء الجلسة المقررة، سيتم التواصل معك لتحديد موعد جديد.', 0, NULL, '2026-06-23 12:00:00'),
('4E064CC0-8EA7-4A7D-8DE1-CC75ACCB55BB', 'E64A2B73-C807-421A-BBDE-A9FCA7D29587', N'SessionCancelled', N'تم إلغاء الجلسة', N'تم إلغاء الجلسة المقررة، سيتم التواصل معك لتحديد موعد جديد.', 1, '2026-06-07 17:47:00', '2026-06-07 12:00:00'),
('010A1A3A-88CD-4932-8EF3-79F8C199247B', 'E64A2B73-C807-421A-BBDE-A9FCA7D29587', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة علاجية مجدولة قريباً، يرجى الحضور في الموعد.', 1, '2026-05-28 18:33:00', '2026-05-28 12:00:00'),
('52737E35-FA10-41C2-9130-278A7025FCBF', 'E64A2B73-C807-421A-BBDE-A9FCA7D29587', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة علاجية مجدولة قريباً، يرجى الحضور في الموعد.', 1, '2026-06-23 17:58:00', '2026-06-23 12:00:00'),
('DA06FF70-B19E-499A-8959-036C3D8B5BFE', 'B672F83A-8C74-4251-BB3F-AF46E9017E52', N'ExerciseReminder', N'تذكير بتمرين', N'حان وقت أداء التمرين المخصص لك اليوم.', 0, NULL, '2026-06-22 12:00:00'),
('E697A554-3C46-42A4-ACBF-B825A03D42BF', 'B672F83A-8C74-4251-BB3F-AF46E9017E52', N'AssessmentDue', N'تقييم جديد متاح', N'تم إتاحة تقييم نفسي جديد لمراجعته وتعبئته.', 1, '2026-06-20 14:29:00', '2026-06-20 12:00:00'),
('9FA1FDCE-E14F-4724-B9B5-F4C9754F3163', 'B672F83A-8C74-4251-BB3F-AF46E9017E52', N'SessionCancelled', N'تم إلغاء الجلسة', N'تم إلغاء الجلسة المقررة، سيتم التواصل معك لتحديد موعد جديد.', 0, NULL, '2026-06-18 12:00:00'),
('A32EB511-AFD3-4633-8411-AA620FB0D9E2', 'B672F83A-8C74-4251-BB3F-AF46E9017E52', N'SessionCancelled', N'تم إلغاء الجلسة', N'تم إلغاء الجلسة المقررة، سيتم التواصل معك لتحديد موعد جديد.', 1, '2026-06-22 15:17:00', '2026-06-22 12:00:00'),
('C02C074D-FEE8-4D1E-9AD7-6DC56A9CC964', 'C16CE25F-85AE-4B1D-BDF1-05FCAAE4FE41', N'WeeklyProgressReport', N'تقرير التقدم الأسبوعي', N'تقرير تقدمك الأسبوعي جاهز الآن للمراجعة.', 1, '2026-06-26 16:23:00', '2026-06-26 12:00:00'),
('6307C4FC-0432-4813-A618-09BBC7CF0734', 'C16CE25F-85AE-4B1D-BDF1-05FCAAE4FE41', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة علاجية مجدولة قريباً، يرجى الحضور في الموعد.', 0, NULL, '2026-05-26 12:00:00'),
('4A12E047-2A59-4F66-A82E-263878DEAB13', 'C16CE25F-85AE-4B1D-BDF1-05FCAAE4FE41', N'AssessmentDue', N'تقييم جديد متاح', N'تم إتاحة تقييم نفسي جديد لمراجعته وتعبئته.', 1, '2026-06-24 21:30:00', '2026-06-24 12:00:00'),
('8650D1EA-8ECF-4DE7-A564-FC867C4C498E', 'C16CE25F-85AE-4B1D-BDF1-05FCAAE4FE41', N'ExerciseReminder', N'تذكير بتمرين', N'حان وقت أداء التمرين المخصص لك اليوم.', 1, '2026-07-03 14:04:00', '2026-07-03 12:00:00'),
('642EBBED-B688-449C-B9C2-A146EFA84182', 'C16CE25F-85AE-4B1D-BDF1-05FCAAE4FE41', N'ExerciseReminder', N'تذكير بتمرين', N'حان وقت أداء التمرين المخصص لك اليوم.', 0, NULL, '2026-05-25 12:00:00'),
('BD64BD9A-ED19-441F-A33F-13B15D8334FC', 'C16CE25F-85AE-4B1D-BDF1-05FCAAE4FE41', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة علاجية مجدولة قريباً، يرجى الحضور في الموعد.', 0, NULL, '2026-07-04 12:00:00'),
('9BB5610E-CA5C-485C-84E4-46AFA45AD5F0', 'C7A4E929-6F01-4043-B3F3-D1A01505C894', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة علاجية مجدولة قريباً، يرجى الحضور في الموعد.', 0, NULL, '2026-05-28 12:00:00'),
('B0643238-247D-4838-9DA5-136C6A7737EC', 'C7A4E929-6F01-4043-B3F3-D1A01505C894', N'SessionCancelled', N'تم إلغاء الجلسة', N'تم إلغاء الجلسة المقررة، سيتم التواصل معك لتحديد موعد جديد.', 1, '2026-05-25 15:10:00', '2026-05-25 12:00:00'),
('893AF8E1-1167-4FB1-8094-39D7B18EBF1E', 'C7A4E929-6F01-4043-B3F3-D1A01505C894', N'AssessmentDue', N'تقييم جديد متاح', N'تم إتاحة تقييم نفسي جديد لمراجعته وتعبئته.', 0, NULL, '2026-06-05 12:00:00'),
('707439A2-2131-4CFA-83F8-EF5CD5FCA4B6', 'C7A4E929-6F01-4043-B3F3-D1A01505C894', N'AssessmentDue', N'تقييم جديد متاح', N'تم إتاحة تقييم نفسي جديد لمراجعته وتعبئته.', 0, NULL, '2026-06-30 12:00:00'),
('F151CBA1-DB9E-40AF-8536-866859698D6E', 'C7A4E929-6F01-4043-B3F3-D1A01505C894', N'ExerciseReminder', N'تذكير بتمرين', N'حان وقت أداء التمرين المخصص لك اليوم.', 0, NULL, '2026-06-10 12:00:00'),
('5AF27C86-BAE5-4653-A699-93497CC3DE5B', 'C7A4E929-6F01-4043-B3F3-D1A01505C894', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة علاجية مجدولة قريباً، يرجى الحضور في الموعد.', 0, NULL, '2026-07-01 12:00:00'),
('E3D96084-A30B-4DF7-8465-D4CF5BBC1901', 'C7A4E929-6F01-4043-B3F3-D1A01505C894', N'WeeklyProgressReport', N'تقرير التقدم الأسبوعي', N'تقرير تقدمك الأسبوعي جاهز الآن للمراجعة.', 1, '2026-06-03 15:06:00', '2026-06-03 12:00:00'),
('7901ABDF-C007-4497-9709-7E8F8DD20BF8', 'C7A4E929-6F01-4043-B3F3-D1A01505C894', N'SessionCancelled', N'تم إلغاء الجلسة', N'تم إلغاء الجلسة المقررة، سيتم التواصل معك لتحديد موعد جديد.', 1, '2026-05-26 18:38:00', '2026-05-26 12:00:00'),
('76A99C52-43FA-49A2-AA2E-8C2AD7A7BA6C', '6F7A2849-8BFC-45CB-83E5-F305BF4B7ADF', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة علاجية مجدولة قريباً، يرجى الحضور في الموعد.', 1, '2026-06-03 16:33:00', '2026-06-03 12:00:00'),
('E070FBF4-DBAC-44EB-B184-216C10B123B7', '6F7A2849-8BFC-45CB-83E5-F305BF4B7ADF', N'ExerciseReminder', N'تذكير بتمرين', N'حان وقت أداء التمرين المخصص لك اليوم.', 1, '2026-06-28 15:24:00', '2026-06-28 12:00:00'),
('C844D2CB-5A46-48C3-A3A9-C5A9B81848E6', '6F7A2849-8BFC-45CB-83E5-F305BF4B7ADF', N'ExerciseReminder', N'تذكير بتمرين', N'حان وقت أداء التمرين المخصص لك اليوم.', 1, '2026-06-25 16:39:00', '2026-06-25 12:00:00'),
('EADE2CA7-0FA7-46E6-82C8-A330A86A5399', '6F7A2849-8BFC-45CB-83E5-F305BF4B7ADF', N'WeeklyProgressReport', N'تقرير التقدم الأسبوعي', N'تقرير تقدمك الأسبوعي جاهز الآن للمراجعة.', 1, '2026-06-09 20:34:00', '2026-06-09 12:00:00'),
('07EBE707-1078-4E05-86A1-70612129D926', '6F7A2849-8BFC-45CB-83E5-F305BF4B7ADF', N'SessionCancelled', N'تم إلغاء الجلسة', N'تم إلغاء الجلسة المقررة، سيتم التواصل معك لتحديد موعد جديد.', 0, NULL, '2026-05-25 12:00:00'),
('963B6827-715A-4491-9629-93725F8A173F', '6F7A2849-8BFC-45CB-83E5-F305BF4B7ADF', N'ExerciseReminder', N'تذكير بتمرين', N'حان وقت أداء التمرين المخصص لك اليوم.', 0, NULL, '2026-06-04 12:00:00'),
('3FDE13E7-5369-4AAA-8C8A-FCA817587BFC', '43FC9F53-CC9C-4DCF-8670-68CA91084378', N'AssessmentDue', N'تقييم جديد متاح', N'تم إتاحة تقييم نفسي جديد لمراجعته وتعبئته.', 1, '2026-06-10 12:51:00', '2026-06-10 12:00:00'),
('7D1AD49F-4D75-424D-8E6B-D9C926B54D4E', '43FC9F53-CC9C-4DCF-8670-68CA91084378', N'SessionCancelled', N'تم إلغاء الجلسة', N'تم إلغاء الجلسة المقررة، سيتم التواصل معك لتحديد موعد جديد.', 0, NULL, '2026-06-14 12:00:00'),
('DDD825A6-C403-4677-8F93-769BB620B979', '43FC9F53-CC9C-4DCF-8670-68CA91084378', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة علاجية مجدولة قريباً، يرجى الحضور في الموعد.', 0, NULL, '2026-06-02 12:00:00'),
('3FC31D12-4B1C-4E85-8C90-92C95F81C9B7', '43FC9F53-CC9C-4DCF-8670-68CA91084378', N'SessionCancelled', N'تم إلغاء الجلسة', N'تم إلغاء الجلسة المقررة، سيتم التواصل معك لتحديد موعد جديد.', 1, '2026-06-11 14:52:00', '2026-06-11 12:00:00'),
('2A3BDDF0-AA1F-4CED-945D-2A8D294A5603', '43FC9F53-CC9C-4DCF-8670-68CA91084378', N'SessionCancelled', N'تم إلغاء الجلسة', N'تم إلغاء الجلسة المقررة، سيتم التواصل معك لتحديد موعد جديد.', 1, '2026-06-19 16:02:00', '2026-06-19 12:00:00'),
('7FFA64A7-8151-46C2-9007-569BD62080FB', '43FC9F53-CC9C-4DCF-8670-68CA91084378', N'ExerciseReminder', N'تذكير بتمرين', N'حان وقت أداء التمرين المخصص لك اليوم.', 1, '2026-06-30 18:52:00', '2026-06-30 12:00:00'),
('1A6F1CA3-CDE4-4A9E-A521-2FD8DED537E9', '43FC9F53-CC9C-4DCF-8670-68CA91084378', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة علاجية مجدولة قريباً، يرجى الحضور في الموعد.', 1, '2026-06-18 14:47:00', '2026-06-18 12:00:00'),
('99FCEABB-514D-4242-AA5D-5C24DDF6CFFD', '4F2CF45C-16B5-4BB6-85BE-3F63C6D1091C', N'ExerciseReminder', N'تذكير بتمرين', N'حان وقت أداء التمرين المخصص لك اليوم.', 1, '2026-05-26 16:31:00', '2026-05-26 12:00:00'),
('1EF3863D-D24B-465D-9DD4-1E21921A3C41', '4F2CF45C-16B5-4BB6-85BE-3F63C6D1091C', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة علاجية مجدولة قريباً، يرجى الحضور في الموعد.', 0, NULL, '2026-05-30 12:00:00'),
('C5B0C2B3-4826-4420-BB5F-BAD1CAD4A2CB', '4F2CF45C-16B5-4BB6-85BE-3F63C6D1091C', N'SessionCancelled', N'تم إلغاء الجلسة', N'تم إلغاء الجلسة المقررة، سيتم التواصل معك لتحديد موعد جديد.', 0, NULL, '2026-06-09 12:00:00'),
('9BDBACEF-3DA8-42B4-98B6-00E99D4CE2DB', 'C652B582-2B56-43AC-ABF6-A78D796BBD93', N'AssessmentDue', N'تقييم جديد متاح', N'تم إتاحة تقييم نفسي جديد لمراجعته وتعبئته.', 1, '2026-06-22 13:23:00', '2026-06-22 12:00:00'),
('6BA5FB15-AABA-4147-8D23-BFC60D283689', 'C652B582-2B56-43AC-ABF6-A78D796BBD93', N'AssessmentDue', N'تقييم جديد متاح', N'تم إتاحة تقييم نفسي جديد لمراجعته وتعبئته.', 1, '2026-06-18 13:42:00', '2026-06-18 12:00:00'),
('A2A8CF79-5840-4A2A-B758-FE33C3EC9907', 'C652B582-2B56-43AC-ABF6-A78D796BBD93', N'AssessmentDue', N'تقييم جديد متاح', N'تم إتاحة تقييم نفسي جديد لمراجعته وتعبئته.', 1, '2026-06-22 16:10:00', '2026-06-22 12:00:00'),
('D613EA37-573E-45D6-BFEC-689C3F4B71F0', 'C652B582-2B56-43AC-ABF6-A78D796BBD93', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة علاجية مجدولة قريباً، يرجى الحضور في الموعد.', 0, NULL, '2026-06-26 12:00:00'),
('4C63BDF6-AEB5-4607-8C99-34A6D97BE967', 'C652B582-2B56-43AC-ABF6-A78D796BBD93', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة علاجية مجدولة قريباً، يرجى الحضور في الموعد.', 1, '2026-06-10 12:46:00', '2026-06-10 12:00:00'),
('171AD6FF-7398-469E-BAA5-AAFA818EAC1C', 'C652B582-2B56-43AC-ABF6-A78D796BBD93', N'AssessmentDue', N'تقييم جديد متاح', N'تم إتاحة تقييم نفسي جديد لمراجعته وتعبئته.', 0, NULL, '2026-06-07 12:00:00'),
('41DC75DB-AD86-48CB-9DFB-82FA289FB7CF', '9F374DC9-631C-4155-B308-FA58C6FDB75B', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة علاجية مجدولة قريباً، يرجى الحضور في الموعد.', 1, '2026-05-23 16:31:00', '2026-05-23 12:00:00'),
('CD4F2E5F-16B2-4C30-BEFC-5B59B91B5456', '9F374DC9-631C-4155-B308-FA58C6FDB75B', N'WeeklyProgressReport', N'تقرير التقدم الأسبوعي', N'تقرير تقدمك الأسبوعي جاهز الآن للمراجعة.', 0, NULL, '2026-05-25 12:00:00'),
('A922270D-C16E-406E-A5F9-B36C855DCC83', '9F374DC9-631C-4155-B308-FA58C6FDB75B', N'ExerciseReminder', N'تذكير بتمرين', N'حان وقت أداء التمرين المخصص لك اليوم.', 1, '2026-07-03 19:14:00', '2026-07-03 12:00:00'),
('AB6B9E71-1E48-41EE-A8DD-BB7AAC94A057', '9F374DC9-631C-4155-B308-FA58C6FDB75B', N'ExerciseReminder', N'تذكير بتمرين', N'حان وقت أداء التمرين المخصص لك اليوم.', 1, '2026-07-04 21:43:00', '2026-07-04 12:00:00'),
('98EEDB9F-46FA-4C72-B250-8F7037BC0349', '9F374DC9-631C-4155-B308-FA58C6FDB75B', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة علاجية مجدولة قريباً، يرجى الحضور في الموعد.', 0, NULL, '2026-07-04 12:00:00'),
('5F129B0E-25EA-4484-973D-876C9CED931E', '9F374DC9-631C-4155-B308-FA58C6FDB75B', N'AssessmentDue', N'تقييم جديد متاح', N'تم إتاحة تقييم نفسي جديد لمراجعته وتعبئته.', 0, NULL, '2026-06-30 12:00:00'),
('C0F895FD-72E4-4989-81B4-5F14B4A97CAD', '9F374DC9-631C-4155-B308-FA58C6FDB75B', N'ExerciseReminder', N'تذكير بتمرين', N'حان وقت أداء التمرين المخصص لك اليوم.', 1, '2026-05-26 21:37:00', '2026-05-26 12:00:00'),
('2308B0AB-2A01-4208-AED3-970967D37CB1', '1C257B5A-D9E6-410F-9BAC-1744E71E1FE7', N'SessionCancelled', N'تم إلغاء الجلسة', N'تم إلغاء الجلسة المقررة، سيتم التواصل معك لتحديد موعد جديد.', 0, NULL, '2026-06-11 12:00:00'),
('72ADCC55-1E2D-4D1E-8FCB-87784D4A1669', '1C257B5A-D9E6-410F-9BAC-1744E71E1FE7', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة علاجية مجدولة قريباً، يرجى الحضور في الموعد.', 1, '2026-06-04 21:41:00', '2026-06-04 12:00:00'),
('AE4B6286-4EA2-42C8-91A3-9C620525C861', '1C257B5A-D9E6-410F-9BAC-1744E71E1FE7', N'SessionCancelled', N'تم إلغاء الجلسة', N'تم إلغاء الجلسة المقررة، سيتم التواصل معك لتحديد موعد جديد.', 0, NULL, '2026-06-01 12:00:00'),
('F6F0A073-78C0-4450-8CE2-A8D38C909C13', '1C257B5A-D9E6-410F-9BAC-1744E71E1FE7', N'ExerciseReminder', N'تذكير بتمرين', N'حان وقت أداء التمرين المخصص لك اليوم.', 1, '2026-07-06 19:45:00', '2026-07-06 12:00:00'),
('7C0F66BF-FFE7-4299-9342-2647FDFDD5F2', '1C257B5A-D9E6-410F-9BAC-1744E71E1FE7', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة علاجية مجدولة قريباً، يرجى الحضور في الموعد.', 0, NULL, '2026-06-11 12:00:00'),
('3CCE7995-C68C-48EB-AEEF-DACAC53866F0', 'F7F7FD61-459C-402D-A380-067F51857677', N'ExerciseReminder', N'تذكير بتمرين', N'حان وقت أداء التمرين المخصص لك اليوم.', 1, '2026-05-22 14:58:00', '2026-05-22 12:00:00'),
('9ECA8FD5-79CE-490A-9B32-B062EBAB00A4', 'F7F7FD61-459C-402D-A380-067F51857677', N'AssessmentDue', N'تقييم جديد متاح', N'تم إتاحة تقييم نفسي جديد لمراجعته وتعبئته.', 0, NULL, '2026-05-25 12:00:00'),
('D1DD27E6-ED71-4F66-9A10-0FA742A6009A', 'F7F7FD61-459C-402D-A380-067F51857677', N'SessionCancelled', N'تم إلغاء الجلسة', N'تم إلغاء الجلسة المقررة، سيتم التواصل معك لتحديد موعد جديد.', 1, '2026-05-30 20:15:00', '2026-05-30 12:00:00'),
('71A72884-36F6-40B7-8267-9BE55C610A80', 'F7F7FD61-459C-402D-A380-067F51857677', N'AssessmentDue', N'تقييم جديد متاح', N'تم إتاحة تقييم نفسي جديد لمراجعته وتعبئته.', 0, NULL, '2026-06-21 12:00:00'),
('555B0B50-C632-4B7E-88DD-0FBE9FB76858', 'F7F7FD61-459C-402D-A380-067F51857677', N'AssessmentDue', N'تقييم جديد متاح', N'تم إتاحة تقييم نفسي جديد لمراجعته وتعبئته.', 0, NULL, '2026-05-26 12:00:00'),
('CBB4B3DF-2A7F-4193-8CD7-FDAB3A40D0FB', 'F7F7FD61-459C-402D-A380-067F51857677', N'ExerciseReminder', N'تذكير بتمرين', N'حان وقت أداء التمرين المخصص لك اليوم.', 0, NULL, '2026-06-20 12:00:00'),
('C4C4931D-75F7-4C06-B053-729F93019588', 'F7F7FD61-459C-402D-A380-067F51857677', N'SessionCancelled', N'تم إلغاء الجلسة', N'تم إلغاء الجلسة المقررة، سيتم التواصل معك لتحديد موعد جديد.', 1, '2026-07-02 18:44:00', '2026-07-02 12:00:00'),
('3828570F-EA4A-4F4D-82D7-AC7A8864FC74', 'F7F7FD61-459C-402D-A380-067F51857677', N'SessionCancelled', N'تم إلغاء الجلسة', N'تم إلغاء الجلسة المقررة، سيتم التواصل معك لتحديد موعد جديد.', 1, '2026-06-17 19:52:00', '2026-06-17 12:00:00'),
('D855E155-0ABC-458A-B7D1-B082F2FBBF21', '9F654540-0CB3-4374-AACF-0E72E6957C1E', N'AssessmentDue', N'تقييم جديد متاح', N'تم إتاحة تقييم نفسي جديد لمراجعته وتعبئته.', 0, NULL, '2026-05-28 12:00:00'),
('68D16298-CB3A-4C7B-8EF4-C9B2BC575F52', '9F654540-0CB3-4374-AACF-0E72E6957C1E', N'AssessmentDue', N'تقييم جديد متاح', N'تم إتاحة تقييم نفسي جديد لمراجعته وتعبئته.', 1, '2026-06-21 20:08:00', '2026-06-21 12:00:00'),
('08A2A578-A8D4-4D54-B7A2-2C33069D94C6', '9F654540-0CB3-4374-AACF-0E72E6957C1E', N'SessionCancelled', N'تم إلغاء الجلسة', N'تم إلغاء الجلسة المقررة، سيتم التواصل معك لتحديد موعد جديد.', 1, '2026-06-28 21:46:00', '2026-06-28 12:00:00'),
('5F7DFEDB-D8AB-46D6-9F8C-BB64F06E6022', '9F654540-0CB3-4374-AACF-0E72E6957C1E', N'AssessmentDue', N'تقييم جديد متاح', N'تم إتاحة تقييم نفسي جديد لمراجعته وتعبئته.', 1, '2026-06-28 17:43:00', '2026-06-28 12:00:00'),
('DE2B796D-45FB-4CDD-9505-CD55C7E36DE1', '9F654540-0CB3-4374-AACF-0E72E6957C1E', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة علاجية مجدولة قريباً، يرجى الحضور في الموعد.', 1, '2026-06-25 21:48:00', '2026-06-25 12:00:00'),
('9697B3DA-6D12-48CA-95E1-C2E75A8F20F2', '9F654540-0CB3-4374-AACF-0E72E6957C1E', N'AssessmentDue', N'تقييم جديد متاح', N'تم إتاحة تقييم نفسي جديد لمراجعته وتعبئته.', 0, NULL, '2026-06-25 12:00:00'),
('6FB923EE-7AC3-44A3-927F-BCAB2FAD859C', '9F654540-0CB3-4374-AACF-0E72E6957C1E', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة علاجية مجدولة قريباً، يرجى الحضور في الموعد.', 1, '2026-06-27 18:21:00', '2026-06-27 12:00:00'),
('4CE808C9-5C46-4344-B9CE-907E840A1B29', '1B4874E3-F13E-4B41-96F2-A4DF4E747070', N'ExerciseReminder', N'تذكير بتمرين', N'حان وقت أداء التمرين المخصص لك اليوم.', 1, '2026-06-04 16:24:00', '2026-06-04 12:00:00'),
('F42EE52C-9FEF-4969-B5E0-C9B71D1FEFF3', '1B4874E3-F13E-4B41-96F2-A4DF4E747070', N'WeeklyProgressReport', N'تقرير التقدم الأسبوعي', N'تقرير تقدمك الأسبوعي جاهز الآن للمراجعة.', 0, NULL, '2026-06-12 12:00:00'),
('728A177E-C1ED-469D-85FA-8D3C777038DF', '1B4874E3-F13E-4B41-96F2-A4DF4E747070', N'ExerciseReminder', N'تذكير بتمرين', N'حان وقت أداء التمرين المخصص لك اليوم.', 1, '2026-07-03 17:14:00', '2026-07-03 12:00:00'),
('214F2B53-9DC9-4131-8D93-2FE4AAB576C1', '1B4874E3-F13E-4B41-96F2-A4DF4E747070', N'ExerciseReminder', N'تذكير بتمرين', N'حان وقت أداء التمرين المخصص لك اليوم.', 1, '2026-06-11 19:28:00', '2026-06-11 12:00:00'),
('91E3327D-7E98-4EFB-BACC-3C01072A22C8', '1B4874E3-F13E-4B41-96F2-A4DF4E747070', N'SessionCancelled', N'تم إلغاء الجلسة', N'تم إلغاء الجلسة المقررة، سيتم التواصل معك لتحديد موعد جديد.', 1, '2026-06-30 12:18:00', '2026-06-30 12:00:00'),
('A6C22D8A-560F-4182-A1F9-2980271A2F0F', '1B4874E3-F13E-4B41-96F2-A4DF4E747070', N'WeeklyProgressReport', N'تقرير التقدم الأسبوعي', N'تقرير تقدمك الأسبوعي جاهز الآن للمراجعة.', 0, NULL, '2026-06-17 12:00:00'),
('2273438E-80B4-4D90-8C14-0EB182ABCE88', '22518D39-E6EC-4742-82D4-A9A14410326C', N'AssessmentDue', N'تقييم جديد متاح', N'تم إتاحة تقييم نفسي جديد لمراجعته وتعبئته.', 1, '2026-05-25 21:13:00', '2026-05-25 12:00:00'),
('CC5755DD-8063-4EB5-B605-3128BBF914F4', '22518D39-E6EC-4742-82D4-A9A14410326C', N'AssessmentDue', N'تقييم جديد متاح', N'تم إتاحة تقييم نفسي جديد لمراجعته وتعبئته.', 1, '2026-07-06 19:40:00', '2026-07-06 12:00:00'),
('BD4C49C4-1C7E-4F01-8B48-6A329D638095', '22518D39-E6EC-4742-82D4-A9A14410326C', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة علاجية مجدولة قريباً، يرجى الحضور في الموعد.', 1, '2026-06-12 20:04:00', '2026-06-12 12:00:00'),
('B39C5C25-1D9E-458B-B682-7E632C4A27D0', '22518D39-E6EC-4742-82D4-A9A14410326C', N'SessionCancelled', N'تم إلغاء الجلسة', N'تم إلغاء الجلسة المقررة، سيتم التواصل معك لتحديد موعد جديد.', 1, '2026-06-12 19:30:00', '2026-06-12 12:00:00'),
('3D45F36D-8280-4F23-AF71-5C2372408624', '22518D39-E6EC-4742-82D4-A9A14410326C', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة علاجية مجدولة قريباً، يرجى الحضور في الموعد.', 0, NULL, '2026-06-25 12:00:00'),
('C0E3AACC-F49E-437B-9DEB-EB70F3EF1AB9', '22518D39-E6EC-4742-82D4-A9A14410326C', N'ExerciseReminder', N'تذكير بتمرين', N'حان وقت أداء التمرين المخصص لك اليوم.', 0, NULL, '2026-05-27 12:00:00'),
('B4F69CBC-5043-4ECC-84B7-92DCB602B35D', '22518D39-E6EC-4742-82D4-A9A14410326C', N'AssessmentDue', N'تقييم جديد متاح', N'تم إتاحة تقييم نفسي جديد لمراجعته وتعبئته.', 1, '2026-06-13 12:31:00', '2026-06-13 12:00:00'),
('11670058-8292-4918-9CC3-7C83CA066D04', '38E2BAFB-3323-4DCB-83E0-192A6E91F5DD', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة علاجية مجدولة قريباً، يرجى الحضور في الموعد.', 1, '2026-06-10 17:26:00', '2026-06-10 12:00:00'),
('E14F7C5A-50FB-4E7A-996E-A3E13FEE3743', '38E2BAFB-3323-4DCB-83E0-192A6E91F5DD', N'ExerciseReminder', N'تذكير بتمرين', N'حان وقت أداء التمرين المخصص لك اليوم.', 1, '2026-06-09 15:01:00', '2026-06-09 12:00:00'),
('88F6D25B-E458-4B5A-929E-37D30CB8674D', '38E2BAFB-3323-4DCB-83E0-192A6E91F5DD', N'WeeklyProgressReport', N'تقرير التقدم الأسبوعي', N'تقرير تقدمك الأسبوعي جاهز الآن للمراجعة.', 1, '2026-06-15 15:58:00', '2026-06-15 12:00:00'),
('1BF25E73-4B81-4942-9607-215158C0884F', '38E2BAFB-3323-4DCB-83E0-192A6E91F5DD', N'AssessmentDue', N'تقييم جديد متاح', N'تم إتاحة تقييم نفسي جديد لمراجعته وتعبئته.', 0, NULL, '2026-05-22 12:00:00'),
('086316B4-2D01-42B8-AA19-0966F6AB0518', '38E2BAFB-3323-4DCB-83E0-192A6E91F5DD', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة علاجية مجدولة قريباً، يرجى الحضور في الموعد.', 0, NULL, '2026-05-29 12:00:00'),
('7389903F-0640-490F-A947-4AB9F45D41EA', '38E2BAFB-3323-4DCB-83E0-192A6E91F5DD', N'AssessmentDue', N'تقييم جديد متاح', N'تم إتاحة تقييم نفسي جديد لمراجعته وتعبئته.', 1, '2026-05-26 15:44:00', '2026-05-26 12:00:00'),
('8570CCF8-E5C1-47E6-A5C6-A4065E7053FE', '38E2BAFB-3323-4DCB-83E0-192A6E91F5DD', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة علاجية مجدولة قريباً، يرجى الحضور في الموعد.', 1, '2026-06-06 18:09:00', '2026-06-06 12:00:00'),
('4F7BA9D3-E51B-460A-9501-5BAB065CBA6F', '8C82ABCC-10FE-487B-9F8F-1E502FF589FD', N'SessionReminder', N'تذكير بجلسة قادمة', N'لديك جلسة علاجية مجدولة قريباً، يرجى الحضور في الموعد.', 1, '2026-07-06 21:25:00', '2026-07-06 12:00:00'),
('DCCDF3E2-1673-49A8-9804-9D7B1DD9C60F', '8C82ABCC-10FE-487B-9F8F-1E502FF589FD', N'WeeklyProgressReport', N'تقرير التقدم الأسبوعي', N'تقرير تقدمك الأسبوعي جاهز الآن للمراجعة.', 1, '2026-06-02 15:28:00', '2026-06-02 12:00:00'),
('4FB10670-6411-4CE8-BDE0-CE053DE0B9AA', '8C82ABCC-10FE-487B-9F8F-1E502FF589FD', N'SessionCancelled', N'تم إلغاء الجلسة', N'تم إلغاء الجلسة المقررة، سيتم التواصل معك لتحديد موعد جديد.', 1, '2026-05-29 15:06:00', '2026-05-29 12:00:00'),
('5CC8241B-EB78-4421-82CC-40A3A11630AB', '9CED10E6-0BFB-4D43-9656-019687D99BC4', N'WeeklyProgressReport', N'تقرير التقدم الأسبوعي', N'تقرير تقدمك الأسبوعي جاهز الآن للمراجعة.', 0, NULL, '2026-06-30 12:00:00'),
('ACF0D295-67BC-443E-941C-0F8923EF403F', '9CED10E6-0BFB-4D43-9656-019687D99BC4', N'AssessmentDue', N'تقييم جديد متاح', N'تم إتاحة تقييم نفسي جديد لمراجعته وتعبئته.', 0, NULL, '2026-06-05 12:00:00'),
('B02719D5-48CA-425B-9F76-6FF35390FD0A', '9CED10E6-0BFB-4D43-9656-019687D99BC4', N'SessionCancelled', N'تم إلغاء الجلسة', N'تم إلغاء الجلسة المقررة، سيتم التواصل معك لتحديد موعد جديد.', 0, NULL, '2026-06-14 12:00:00'),
('AE5197E2-E1AB-43C8-ADB5-4D680D4EBA36', '9CED10E6-0BFB-4D43-9656-019687D99BC4', N'WeeklyProgressReport', N'تقرير التقدم الأسبوعي', N'تقرير تقدمك الأسبوعي جاهز الآن للمراجعة.', 1, '2026-07-02 14:32:00', '2026-07-02 12:00:00');

COMMIT TRANSACTION;
PRINT 'Jalsa seed data import complete.';
