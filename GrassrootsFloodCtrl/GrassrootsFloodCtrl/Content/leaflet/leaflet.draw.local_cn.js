/*
 *  leaflet.draw 控件的本地化。
 * 
 *  @author junwei.hu 2015
 */
L.drawLocal = {
    draw: {
        toolbar: {
            actions: {
                title: '取消标绘',
                text: '取消'
            },
            undo: {
                title: '删除最后一个点',
                text: '删除最后一个点'
            },
            buttons: {
                polyline: '画线',
                polygon: '画多边型',
                rectangle: '画正方形',
                circle: '画圆',
                marker: '画点'
            }
        },
        handlers: {
            circle: {
                tooltip: {
                    start: '点击并拖动画圆.'
                },
                radius: '半径'
            },
            marker: {
                tooltip: {
                    start: '点击地图定位点.'
                }
            },
            polygon: {
                tooltip: {
                    start: '点击地图开始画多边型.',
                    cont: '点击继续多边型.',
                    end: '点击第一个点结束画型.'
                }
            },
            polyline: {
                error: '<strong>错误:</strong> 形状边缘不能交叉!',
                tooltip: {
                    start: '点击开始画线.',
                    cont: '点击继续画线.',
                    end: '点击最后一个点结束画线.'
                }
            },
            rectangle: {
                tooltip: {
                    start: '点击拖动画正方形.'
                }
            },
            simpleshape: {
                tooltip: {
                    end: '释放鼠标结束.'
                }
            }
        }
    },
    edit: {
        toolbar: {
            actions: {
                save: {
                    title: '保存变动.',
                    text: '保存'
                },
                cancel: {
                    title: '取消编辑，撤消所有变动.',
                    text: '取消'
                }
            },
            buttons: {
                edit: '编辑层.',
                editDisabled: '没有层可编辑.',
                remove: '删除层.',
                removeDisabled: '没有层可删除.'
            }
        },
        handlers: {
            edit: {
                tooltip: {
                    text: '拖动形状或点来编辑.',
                    subtext: '点击取消撤消变动.'
                }
            },
            remove: {
                tooltip: {
                    text: '点击形状删除'
                }
            }
        }
    }
};
