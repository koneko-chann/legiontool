@echo off
chcp 65001 > nul
echo ====================================================
echo   REPUBLIC LEGION CC - RAID MISSION AUTOMATION TOOL
echo ====================================================
echo.

C:\Python313\python.exe "d:\region tool\republic_raid_bot.py" %*

echo.
echo Hoàn thành! Kiểm tra ảnh chụp màn hình tại thư mục: screenshots
pause
