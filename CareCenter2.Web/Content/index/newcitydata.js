//养老方式
var cityData_1 = [
    {
        "id": "820100",
        "name": "社区养老",
        "parentId": "820000",
        "shortName": "社区养老",
        "letter": "S",
        "cityCode": "00853",
        "pinyin": "MacauPeninsula"
    },
    {
        "id": "820200",
        "name": "居家养老",
        "parentId": "820000",
        "shortName": "居家养老",
        "letter": "J",
        "cityCode": "00853",
        "pinyin": "Taipa"
    },
    {
        "id": "820300",
        "name": "机构养老",
        "parentId": "820000",
        "shortName": "机构养老",
        "letter": "J",
        "cityCode": "00853",
        "pinyin": "Coloane"
    },
    {
        "id": "820400",
        "name": "旅游养老",
        "parentId": "820000",
        "shortName": "旅游养老",
        "letter": "L",
        "cityCode": "00853",
        "pinyin": "Coloane"
    }
];
//身体情况
var cityData_2 = [
    {
        "id": "820100",
        "name": "失能",
        "parentId": "820000",
        "shortName": "失能",
        "letter": "S",
        "cityCode": "00853",
        "pinyin": "MacauPeninsula"
    },
    {
        "id": "820200",
        "name": "半失能",
        "parentId": "820000",
        "shortName": "半失能",
        "letter": "B",
        "cityCode": "00853",
        "pinyin": "Taipa"
    },
    {
        "id": "820300",
        "name": "健康",
        "parentId": "820000",
        "shortName": "健康",
        "letter": "J",
        "cityCode": "00853",
        "pinyin": "Coloane"
    }
];
//日照护方式
var cityData_3 = [
    {
        "id": "820100",
        "name": "4h(半天)",
        "parentId": "820000",
        "shortName": "4h(半天)",
        "letter": "B",
        "cityCode": "00853",
        "pinyin": "MacauPeninsula"
    },
    {
        "id": "820200",
        "name": "10h(白天)",
        "parentId": "820000",
        "shortName": "10h(白天)",
        "letter": "B",
        "cityCode": "00853",
        "pinyin": "Taipa"
    },
    {
        "id": "820300",
        "name": "24h(全天)",
        "parentId": "820000",
        "shortName": "24h(全天)",
        "letter": "Q",
        "cityCode": "00853",
        "pinyin": "Coloane"
    }
];
//照护地点
var cityData_4 = [
    {
        "id": "820100",
        "name": "居家",
        "parentId": "820000",
        "shortName": "居家",
        "letter": "J",
        "cityCode": "00853",
        "pinyin": "MacauPeninsula"
    },
    {
        "id": "820200",
        "name": "定点社区",
        "parentId": "820000",
        "shortName": "定点社区",
        "letter": "D",
        "cityCode": "00853",
        "pinyin": "Taipa"
    },
    {
        "id": "820300",
        "name": "机构养老",
        "parentId": "820000",
        "shortName": "机构养老",
        "letter": "J",
        "cityCode": "00853",
        "pinyin": "Coloane"
    },
    {
        "id": "820400",
        "name": "旅游养老",
        "parentId": "820000",
        "shortName": "旅游养老",
        "letter": "L",
        "cityCode": "00853",
        "pinyin": "Coloane"
    }
];
//照护人数
var cityData_5 = [
    {
        "id": "820100",
        "name": "1人",
        "parentId": "820000",
        "shortName": "1人",
        "letter": "A",
        "cityCode": "00853",
        "pinyin": "MacauPeninsula"
    },
    {
        "id": "820200",
        "name": "2人",
        "parentId": "820000",
        "shortName": "2人",
        "letter": "B",
        "cityCode": "00853",
        "pinyin": "Taipa"
    },
    {
        "id": "820300",
        "name": "3人",
        "parentId": "820000",
        "shortName": "3人",
        "letter": "S",
        "cityCode": "00853",
        "pinyin": "Coloane"
    }
    ,
    {
        "id": "820300",
        "name": "4人",
        "parentId": "820000",
        "shortName": "4人",
        "letter": "S",
        "cityCode": "00853",
        "pinyin": "Coloane"
    }
    ,
    {
        "id": "820300",
        "name": "5人",
        "parentId": "820000",
        "shortName": "5人",
        "letter": "W",
        "cityCode": "00853",
        "pinyin": "Coloane"
    }
];

//婴幼照护
var cityData_6 = [
    {
        "id": "820100",
        "name": "婴儿期宝宝",
        "parentId": "820000",
        "shortName": "婴儿期宝宝",
        "letter": "Y",
        "cityCode": "00853",
        "pinyin": "MacauPeninsula"
    },
    {
        "id": "820200",
        "name": "幼儿期宝宝",
        "parentId": "820000",
        "shortName": "幼儿期宝宝",
        "letter": "Y",
        "cityCode": "00853",
        "pinyin": "Taipa"
    },
    {
        "id": "820300",
        "name": "学前期宝宝",
        "parentId": "820000",
        "shortName": "学前期宝宝",
        "letter": "X",
        "cityCode": "00853",
        "pinyin": "Coloane"
    }
];
//照护地点(少)
var cityData_7 = [
    {
        "id": "820100",
        "name": "居家",
        "parentId": "820000",
        "shortName": "居家",
        "letter": "J",
        "cityCode": "00853",
        "pinyin": "MacauPeninsula"
    },
    {
        "id": "820200",
        "name": "定点社区",
        "parentId": "820000",
        "shortName": "定点社区",
        "letter": "D",
        "cityCode": "00853",
        "pinyin": "Taipa"
    }
];
//孕产照护
var cityData_8 = [
    {
        "id": "820100",
        "name": "孕期照护",
        "parentId": "820000",
        "shortName": "孕期照护",
        "letter": "Y",
        "cityCode": "00853",
        "pinyin": "MacauPeninsula"
    },
    {
        "id": "820200",
        "name": "产期照护",
        "parentId": "820000",
        "shortName": "产期照护",
        "letter": "C",
        "cityCode": "00853",
        "pinyin": "Taipa"
    },
    {
        "id": "820300",
        "name": "围褥期照护",
        "parentId": "820000",
        "shortName": "围褥期照护",
        "letter": "W",
        "cityCode": "00853",
        "pinyin": "Coloane"
    }
];
//课后照护
var cityData_9 = [
    {
        "id": "820100",
        "name": "白天照护",
        "parentId": "820000",
        "shortName": "白天照护",
        "letter": "B",
        "cityCode": "00853",
        "pinyin": "MacauPeninsula"
    },
    {
        "id": "820200",
        "name": "夜晚照护",
        "parentId": "820000",
        "shortName": "夜晚照护",
        "letter": "Y",
        "cityCode": "00853",
        "pinyin": "Taipa"
    },
    {
        "id": "820300",
        "name": "节假日照护",
        "parentId": "820000",
        "shortName": "节假日照护",
        "letter": "J",
        "cityCode": "00853",
        "pinyin": "Coloane"
    },
    {
        "id": "820400",
        "name": "全天照护",
        "parentId": "820000",
        "shortName": "全天照护",
        "letter": "J",
        "cityCode": "00853",
        "pinyin": "Coloane"
    }
];