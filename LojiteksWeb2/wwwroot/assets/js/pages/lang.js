(function ($) {
    'use strict';

    var language = localStorage.getItem('language');
    var default_lang = 'tr';

    function setLanguage(lang) {
        if (document.getElementById("header-lang-img")) {
            var flagMap = {
                en: "/assets/images/flags/us.jpg",
                sp: "/assets/images/flags/spain.jpg",
                gr: "/assets/images/flags/germany.jpg",
                it: "/assets/images/flags/italy.jpg",
                ru: "/assets/images/flags/russia.jpg",
                tr: "/assets/images/flags/turkey.jpg"
            };

            document.getElementById("header-lang-img").src = flagMap[lang] || flagMap.tr;
            localStorage.setItem('language', lang);
            language = localStorage.getItem('language');
            getLanguage();
        }
    }

    function getLanguage() {
        if (!language) {
            setLanguage(default_lang);
        }

        $.getJSON(`/assets/lang/${language}.json`)
            .done(function (lang) {
                $('html').attr('lang', language);
                let pageKey = $("body").data("page") ? $("body").data("page").toLowerCase() : "index";

                if (lang.head && lang.head[`t-title-${pageKey}`]) {
                    document.title = `${lang.head[`t-title-${pageKey}`]} | Lojiteks`;
                    $("meta[name='title']").attr("content", document.title);
                }

                let descriptionKey = `t-desc-${pageKey}`;
                if (lang.head && lang.head[descriptionKey]) {
                    $("meta[name='description']").attr("content", lang.head[descriptionKey]);
                } else {
                    $("meta[name='description']").attr("content", `${lang.head[pageKey] || "Lojiteks"} | Lojiteks`);
                }

                $("[data-key]").each(function () {
                    let key = $(this).data("key");
                    let translation = key.startsWith("head.") ? lang.head[key.replace("head.", "")] : lang[key];

                    if (translation) {
                        $(this).html(translation);
                    } else {
                        console.warn(`Çeviri bulunamadı: ${key}`);
                    }
                });
            })
            .fail(function () {
                console.error(`Dil dosyası yüklenemedi: /assets/lang/${language}.json`);
            });
    }

    function initLanguage() {
        if (language != null && language !== default_lang) {
            document.addEventListener("DOMContentLoaded", function () {
                window.onload = function () {
                    setLanguage(language);
                };
            });
        }

        $('.language').on('click', function () {
            setLanguage($(this).attr('data-lang'));
        });
    }

    $(document).ready(initLanguage);
})(jQuery);
