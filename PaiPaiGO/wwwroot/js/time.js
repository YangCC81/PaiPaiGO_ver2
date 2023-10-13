// 初始設定截止時間，依照需求設定
//var deadline = new Date('October 11, 2023 12:00:00').getTime();

// 更新剩餘時間的函式
//function updateRemainingTime() {
//    var now = new Date().getTime();
//    var timeRemaining = deadline - now;

//     計算剩餘的天、小時、分鐘和秒
//    var days = Math.floor(timeRemaining / (1000 * 60 * 60 * 24));
//    var hours = Math.floor((timeRemaining % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
//    var minutes = Math.floor((timeRemaining % (1000 * 60 * 60)) / (1000 * 60));
//    var seconds = Math.floor((timeRemaining % (1000 * 60)) / 1000);

//     返回剩餘時間的字串
//    return days + ' 天 ' + hours + ' 小時 ' + minutes + ' 分鐘 ' + seconds + ' 秒 ';
//}

// 顯示剩餘時間
//function showRemainingTime() {
//    var timePopup = document.getElementById('timePopup');

//     如果已經顯示，再次點擊時隱藏
//    if (timePopup.style.display === 'block') {
//        timePopup.style.display = 'none';
//    } else {
//         切換為顯示
//        timePopup.style.display = 'block';

//        function updateAndDisplayTime() {
//            var remainingTime = updateRemainingTime();
//            timePopup.innerHTML = '任務剩餘時間: ' + remainingTime;
//        }

//         初始顯示
//        updateAndDisplayTime();

//         每秒更新一次剩餘時間
//        setInterval(updateAndDisplayTime, 1000);
//    }
//}


// 客服的function(尚無法有互動功能)
function openForm() {
    document.getElementById("myForm").style.display = "block";
}

function closeForm() {
    document.getElementById("myForm").style.display = "none";
}

function openForm() {


    // Hide the open button
    document.querySelector(".open-button").style.display = "none";

    // Display the chat form
    document.getElementById("myForm").style.display = "block";
}

function closeForm() {
    // Show the open button
    document.querySelector(".open-button").style.display = "block";

    // Hide the chat form
    document.getElementById("myForm").style.display = "none";
}




//function showRemainingTime() {
//    var timePopup = document.getElementById('timePopup');

//     發送GET請求到後端API
//    fetch('/api/Missions/remaining-time')
//        .then(response => response.text())
//        .then(remainingTime => {
//             更新剩餘時間的顯示
//            timePopup.innerHTML = '任務剩餘時間: ' + remainingTime;
//        })
//        .catch(error => {
//            console.error('Error fetching remaining time:', error);
//        });
//}

//function showCountdown() {
//     使用 JavaScript 在前端實現倒數計時器
//    fetch('/api/Missions/DeadlineDate') // 請將路徑替換為實際的後端端點
//        .then(response => response.json())
//        .then(data => {
//            var DeadlineDate = new Date(data.DeadlineDate).getTime();
//            var timePopupElement = document.getElementById('timePopup');

//            function updateCountdown() {
//                var now = new Date().getTime();
//                var timeRemaining = DeadlineDate - now;

//                var days = Math.floor(timeRemaining / (1000 * 60 * 60 * 24));
//                var hours = Math.floor((timeRemaining % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
//                var minutes = Math.floor((timeRemaining % (1000 * 60 * 60)) / (1000 * 60));
//                var seconds = Math.floor((timeRemaining % (1000 * 60)) / 1000);

//                timePopupElement.textContent = '任務剩餘時間: ' + days + ' 天 ' + hours + ' 小時 ' + minutes + ' 分鐘 ' + seconds + ' 秒 ';
//            }

//             初始顯示
//            updateCountdown();

//             每秒更新一次剩餘時間
//            setInterval(updateCountdown, 1000);
//        })
//        .catch(error => {
//            console.error('Error fetching task time:', error);
//        });
//}

//function GetRemainingTime() {
//    console.log('234');
//    $.ajax({
//        type: 'POST',
//        url: '@Url.Action("GetRemainingTime", "Missions")',
//        data: ,
//        success: function (response) {
//            console.log('123');
//        }
//    });
//};