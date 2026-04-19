const eccentricApp = document.querySelector("#eccentric-app");

function showModal(modalHtml) {
    eccentricApp.innerHTML = `<div id="eccentric-modal">${modalHtml}</div>`;
}

function hideModal() {
    eccentricApp.innerHTML = ``;
}

function showLeaderboard() {
    let title = getLang() === "ru-RU" ? "Таблица лидеров" : "Leaderboard";
    let urlDefaultAvatar = "EccentricData/Icons/icon_player.png";
    let modalHtml = `<div id="modal-leaderboard">
        <div id="modal-leaderboard-top">
            <div class="title">${title}</div>
            <img src="EccentricData/Icons/btn_close.png" class="modal-content-top-btn-close">
        </div>
        <div id="modal-leaderboard-middle">
            <div class="modal-leaderboard-middle-item" id="modal-leaderboard-middle-item-0">
                <div class="modal-leaderboard-middle-item-number"></div>
                <img src=${urlDefaultAvatar} alt="" class="modal-leaderboard-middle-item-avatar">
                <div class="modal-leaderboard-middle-item-name"></div>
                <div class="modal-leaderboard-middle-item-score">
                    <div class="modal-leaderboard-middle-item-score-placeholder">
                        <div class="modal-leaderboard-middle-item-score-placeholder-text">
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal-leaderboard-middle-item" id="modal-leaderboard-middle-item-1">
                <div class="modal-leaderboard-middle-item-number"></div>
                <img src=${urlDefaultAvatar} alt="" class="modal-leaderboard-middle-item-avatar">
                <div class="modal-leaderboard-middle-item-name"></div>
                <div class="modal-leaderboard-middle-item-score">
                    <div class="modal-leaderboard-middle-item-score-placeholder">
                        <div class="modal-leaderboard-middle-item-score-placeholder-text">
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal-leaderboard-middle-item" id="modal-leaderboard-middle-item-2" >
                <div class="modal-leaderboard-middle-item-number"></div>
                <img src=${urlDefaultAvatar} alt="" class="modal-leaderboard-middle-item-avatar">
                <div class="modal-leaderboard-middle-item-name"></div>
                <div class="modal-leaderboard-middle-item-score">
                    <div class="modal-leaderboard-middle-item-score-placeholder">
                        <div class="modal-leaderboard-middle-item-score-placeholder-text">
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal-leaderboard-middle-item" id="modal-leaderboard-middle-item-3">
                <div class="modal-leaderboard-middle-item-number"></div>
                <img src=${urlDefaultAvatar} alt="" class="modal-leaderboard-middle-item-avatar">
                <div class="modal-leaderboard-middle-item-name"></div>
                <div class="modal-leaderboard-middle-item-score">
                    <div class="modal-leaderboard-middle-item-score-placeholder">
                        <div class="modal-leaderboard-middle-item-score-placeholder-text">
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal-leaderboard-middle-item" id="modal-leaderboard-middle-item-4">
                <div class="modal-leaderboard-middle-item-number"></div>
                <img src=${urlDefaultAvatar} alt="" class="modal-leaderboard-middle-item-avatar">
                <div class="modal-leaderboard-middle-item-name"></div>
                <div class="modal-leaderboard-middle-item-score">
                    <div class="modal-leaderboard-middle-item-score-placeholder">
                        <div class="modal-leaderboard-middle-item-score-placeholder-text">
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal-leaderboard-middle-item" id="modal-leaderboard-middle-item-5">
                <div class="modal-leaderboard-middle-item-number"></div>
                <img src=${urlDefaultAvatar} alt="" class="modal-leaderboard-middle-item-avatar">
                <div class="modal-leaderboard-middle-item-name"></div>
                <div class="modal-leaderboard-middle-item-score">
                    <div class="modal-leaderboard-middle-item-score-placeholder">
                        <div class="modal-leaderboard-middle-item-score-placeholder-text">
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal-leaderboard-middle-item" id="modal-leaderboard-middle-item-6">
                <div class="modal-leaderboard-middle-item-number"></div>
                <img src=${urlDefaultAvatar} alt="" class="modal-leaderboard-middle-item-avatar">
                <div class="modal-leaderboard-middle-item-name"></div>
                <div class="modal-leaderboard-middle-item-score">
                    <div class="modal-leaderboard-middle-item-score-placeholder">
                        <div class="modal-leaderboard-middle-item-score-placeholder-text">
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal-leaderboard-middle-item" id="modal-leaderboard-middle-item-7">
                <div class="modal-leaderboard-middle-item-number"></div>
                <img src=${urlDefaultAvatar} alt="" class="modal-leaderboard-middle-item-avatar">
                <div class="modal-leaderboard-middle-item-name"></div>
                <div class="modal-leaderboard-middle-item-score">
                    <div class="modal-leaderboard-middle-item-score-placeholder">
                        <div class="modal-leaderboard-middle-item-score-placeholder-text">
                        </div>
                    </div>
                </div>
            </div>

        </div>
        <div id="modal-leaderboard-bottom"></div>
    </div>`;

    showModal(modalHtml);
    let btnClose = document.querySelector("#modal-leaderboard-top .modal-content-top-btn-close");
    btnClose.addEventListener("click", hideModal);
}

function setLeaderboardData(index, number, avatarUrl, name, score, isPlayer) {
    let items = document.querySelectorAll(".modal-leaderboard-middle-item");
    let placeholders = document.querySelectorAll(".modal-leaderboard-middle-item-score-placeholder");
    let numbers = document.querySelectorAll(".modal-leaderboard-middle-item-number");
    let avatars = document.querySelectorAll(".modal-leaderboard-middle-item-avatar");
    let names = document.querySelectorAll(".modal-leaderboard-middle-item-name");
    let scores = document.querySelectorAll(".modal-leaderboard-middle-item-score-placeholder-text");
    if (isPlayer == true) {
        items[index].classList.add("modal-leaderboard-middle-item-selected");
        placeholders[index].classList.add("modal-leaderboard-middle-item-score-placeholder-selected");
    }

    if (number.toString().length > 3) {
        numbers[index].style.fontSize = "0.8em";
    }
    numbers[index].textContent = number;

    avatars[index].src = avatarUrl;

    names[index].textContent = name;

    if (score.toString().length > 8) {
        scores[index].style.fontSize = "1.2em";
    }
    scores[index].textContent = score;

}

function showPromoGameModal(idGP, idYa, title) {
    let modalHtml = `<div id="modal-promo-game">
    <div class="modal-content">
        <div class="modal-promo-game-content-top">
            <button id="modal-promo-game-btn-close">&times;</button>
        </div>
        <div class="modal-promo-game-content-bottom">
            <div id="modal-promo-game-title" class="title"></div>
            <a id="modal-promo-game-btn-open" href="" target="_blank"></a>
        </div>
    </div>
</div>`;
    showModal(modalHtml);
    let btnOpenPromoGame = document.querySelector("#eccentric-modal #modal-promo-game-btn-open");
    let btnPromoClose = document.querySelector("#eccentric-modal #modal-promo-game-btn-close");
    let titlePromo = document.querySelector("#eccentric-modal #modal-promo-game-title");
    let modalPromoContentTop = document.querySelector("#eccentric-modal .modal-promo-game-content-top");
    let lang = getLang();

    btnPromoClose.addEventListener("click", hideModal);
    btnOpenPromoGame.addEventListener("click", hideModal);
    if (lang === "ru-RU") {
        modalPromoContentTop.style.backgroundImage = `url(https://s3.eponesh.com/games/files/9347/banner_${idGP}_ru.jpg)`;
        btnOpenPromoGame.href = `https://yandex.ru/games/app/${idYa}?lang=ru`;
        btnOpenPromoGame.textContent = "Играть";
    } else {
        modalPromoContentTop.style.backgroundImage = `url(https://s3.eponesh.com/games/files/9347/banner_${idGP}_en.jpg)`;
        btnOpenPromoGame.href = `https://yandex.com/games/app/${idYa}?lang=en`;
        btnOpenPromoGame.textContent = "Play";
    }
    titlePromo.textContent = `${title}`;

}

function showCustomModal(text, isCanClose) {

    let modalHtml = `<div id="modal-custom">
    <div class="modal-content modal-content-custom">
        <div class="modal-custom-content-top">
        </div>
        <div class="modal-custom-content-middle">
            <div id="modal-custom-title" class="title"></div>
        </div>
        <div class="modal-custom-content-bottom">
            <button id="modal-custom-btn-ok">OK</button>
        </div>
    </div>
 </div>`;
    showModal(modalHtml);
    let title = document.querySelector("#eccentric-modal #modal-custom-title");
    let btnOk = document.querySelector("#eccentric-modal #modal-custom-btn-ok");
    title.textContent = text;
    if (isCanClose) {
        btnOk.addEventListener("click", hideModal);
    } else {
        btnOk.style.display = "none";
    }

}


function toggle(modal, enable) {
    if (enable === true) {
        modal.classList.add("show");
        modal.classList.remove("hide");
    } else {
        modal.classList.add("hide");
        modal.classList.remove("show");
    }
}

function showCollectionModal(nameCollection) {
    let modalHtml = `<div id="modal-our-games">
    <div class="modal-content-our-games">
        <img src="EccentricData/Icons/btn_close.png" class="modal-content-top-btn-close">
        <div class="modal-content-our-games-top">
            <div class="modal-content-our-games-top-title">
            </div>
        </div>
        <div class="modal-content-our-games-middle">
            <a href="" target="_blank" class="item-our-games">
                <div class="item-our-games-text"></div>
            </a>
            <a href="" target="_blank" class="item-our-games">
                <div class="item-our-games-text"></div>
            </a>
            <a href="" target="_blank" class="item-our-games">
                <div class="item-our-games-text"></div>
            </a>
            <a href="" target="_blank" class="item-our-games">
                <div class="item-our-games-text"></div>
            </a>
            <a href="" target="_blank" class="item-our-games">
                <div class="item-our-games-text"></div>
            </a>
            <a href="" target="_blank" class="item-our-games">
                <div class="item-our-games-text"></div>
            </a>
        </div>
        <div class="modal-content-our-games-bottom"></div>
    </div>
</div>`;
    showModal(modalHtml);
    let btnClose = document.querySelector("#eccentric-modal .modal-content-our-games .modal-content-top-btn-close");
    let name = document.querySelector("#eccentric-modal .modal-content-our-games-top-title");
    name.textContent = nameCollection;
    btnClose.addEventListener("click", hideModal);
}


function setCollectionData(index, nameGame, link, urlBanner) {
    let items = document.querySelectorAll(".item-our-games");
    let textItems = document.querySelectorAll(".item-our-games-text");

    toggle(items[index], true);
    items[index].style.backgroundImage = `url(${urlBanner})`;
    items[index].href = link;
    textItems[index].textContent = nameGame;

}

function getLang() {
    let platform = window.location.hostname;
    let isYandex = platform.includes("yandex");
    let lang;
    if (isYandex) {
        let href = window.location.href;
        let ruLang = href.includes("lang=ru");
        lang = ruLang ? "ru-RU" : "en-US";
        return lang;
    }
    lang = navigator.language || navigator.userLanguage;
    return lang;
}


function isMobile() {
    const ua = navigator.userAgent;

    if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(ua)) {
        return true;
    } else {
        return false;
    }
}

function getCurrentPlatform() {
    return window.location.hostname;
}
