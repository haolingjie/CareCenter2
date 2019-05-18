function pagination(a) {
	this.container = a.container, this.cons = a.container.find("li"), this.count = this.cons.length, this.prevBtn = a.prevBtn, this.nextBtn = a.nextBtn, this.num = a.num, this.page = Math.ceil(this.count / this.num), this.iPage = 0, this.init()
}
pagination.prototype.init = function() {
	var a, b;
	for (This = this, this.container.css("width", 100 * this.page + "%"), a = 0; a < this.page; a++) this.container.append("<ul></ul>");
	b = this.container.find("ul"), b.css("width", 100 / this.page + "%"), this.cons.each(function(a) {
		var c = parseInt(a / This.num);
		$(this).appendTo(b.eq(c))
	}), this.prevBtn.on("click", function() {
		This._prev()
	}), this.nextBtn.on("click", function() {
		This._next()
	})
}, pagination.prototype._scroll = function() {
	this.container.stop().animate({
		left: -100 * this.iPage + "%"
	})
}, pagination.prototype._prev = function() {
	this.iPage--, this.iPage < 0 && (this.iPage = this.page - 1), this._scroll()
}, pagination.prototype._next = function() {
	this.iPage++, this.iPage > this.page - 1 && (this.iPage = 0), this._scroll()
}, $(document).ready(function() {
	function a() {
		return arguments[0] == f ? !1 : (g = f, null != arguments[0] ? f = arguments[0] : f >= d - 1 ? f = 0 : f++, i.eq(g).attr("class", "flip_front"), setTimeout(function() {
			i.eq(g).css("display", "none"), i.eq(f).attr("class", "flip_back").css("display", "block")
		}, 400), c.stop().animate({
			left: -100 * f + "%"
		}, 1e3, "easeInOutExpo"), $("#focus_dot").find("li").eq(f).addClass("cur").siblings().removeClass(), void 0)
	}

	function b() {
		$("#progress").stop().animate({
			width: "100%"
		}, 1e4, function() {
			$(this).css("width", 0), a(), b()
		})
	}
	var c, d, e, f, g, h, i, j, k, l;
	for ($(window).resize(function() {
			$(".case_wrap").css("paddingTop", $("#viewport").height())
		}), c = $("#focus_main"), d = c.find("li").size(), c.css("width", 100 * d + "%").find("li").css("width", 100 / d + "%"), $("#banner").append('<div id="focus_dot"></div><div id="focus_info"></div>'), e = 0; d > e; e++) 0 == e ? $("#focus_dot").append("<li class='cur'><b></b></li>") : $("#focus_dot").append("<li><b></b></li>");
	c.find("article").appendTo("#focus_info"), f = 0, g = 0, h = 0, i = $("#focus_info").find("article"), i.eq(0).css("display", "block"), $("#focus_dot").find("li").on("click", function() {
		c.is(":animated") || f == $(this).index() || ($("#progress").stop().css("width", 0), h = 1, a($(this).index()))
	}), setTimeout(b, 2500), $("#focus_info, #focus_dot").hover(function() {
		$("#progress").pause()
	}, function() {
		h ? (h = 0, b()) : $("#progress").resume()
	}), new pagination({
		container: $("#viewport"),
		prevBtn: $("#prev"),
		nextBtn: $("#next"),
		num: 8
	}), j = $("#detail").find("li"), k = 0, l = j.size() - 1, j.eq(0).addClass("cur"), $("#detail_prev").on("click", function() {
		k--, 0 > k && (k = l), j.removeClass().eq(k).addClass("cur")
	}), $("#detail_next").on("click", function() {
		k++, k > l && (k = 0), j.removeClass().eq(k).addClass("cur")
	}), $.support.leadingWhitespace || $("#client").find("a").hover(function() {
		$(this).find("img").eq(1).stop().fadeOut()
	}, function() {
		$(this).find("img").eq(1).stop().fadeIn()
	})
}), window.onload = function() {
	function a(a) {
		a.each(function(a) {
			$(window).scrollTop() + $(window).height() >= $(this).offset().top && $(this).attr("off") && $(this).delay(250 * a).animate({
				top: 0,
				opacity: 1
			}, 750, "easeOutQuad", function() {
				$(this).attr("off", !1).find("dd").each(function(a) {
					$(this).delay(200 * a).animate({
						opacity: 1,
						left: 0
					})
				})
			})
		})
	}
	setTimeout(function() {
		$(".case_wrap").css("paddingTop", $("#viewport").height())
	}, 1e3);
	var b = $("#profession_list").find("dl");
	b.size() > 0 && (b.css({
		opacity: 0,
		position: "relative",
		top: 80
	}).attr("off", !0).find("dd").css({
		opacity: 0,
		position: "relative",
		left: -50
	}), $(window).on("scroll", function() {
		a(b)
	}))
}, ! function() {
	var a = document.documentElement,
		b = "orientationchange" in window ? "orientationchange" : "resize",
		c = function() {
			var b = a.clientWidth;
			b && (a.style.fontSize = 20 * (b / 1920) + "px")
		};
	document.addEventListener && (window.addEventListener(b, c, !1), document.addEventListener("DOMContentLoaded", c, !1))
}();
