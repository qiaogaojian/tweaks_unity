import MetaMaskOnboarding from "@metamask/onboarding";
// import { ElLoading } from "element-plus"
import {ethers} from 'ethers';
const {ethereum} = window;
let ethersProvider;

/** TODO 王博
 * 判断当前浏览器是否安装 metamask插件
 */
export const isMetaMaskInstalled = function () {
    let eth = Boolean(ethereum && ethereum.isMetaMask);
    if (eth) {
        ethersProvider = new ethers.providers.Web3Pr ovider(ethereum, 'any');
    }
    return eth;
};

/** TODO 王博
 * 检查当前网站是否连接钱包，如果没有安装钱包则安装，如果安装则连接
 * @param forwarderOrigin 当前网页url
 */
export const MetaMaskClientCheck = async function (forwarderOrigin) {
    if (!isMetaMaskInstalled()) {
        onInstall(forwarderOrigin);
    } else {
        return onConnect();
    }
};

/** TODO 王博
 * 安装metamask浏览器扩展钱包
 * @param forwarderOrigin 当前网址
 */
export const onInstall = (forwarderOrigin) => {
    const onboarding = new MetaMaskOnboarding({ forwarderOrigin });
    onboarding.startOnboarding();
};

/**
 * 连接小狐狸钱包
 */
export const onConnect = async function () {
    try {
        ethereum.request({ method: 'eth_requestAccounts' }).then(res => {
            console.log(res)
        }, (err) => {
            // console.log(err)
            alert('Please link wallet')
        })
        // let result = await ethereum.request({ method: 'eth_requestAccounts' });
        // return result;
    } catch (error) {
        if (error.code == 4001) {
            alert('User rejected the request.')
        }
    }
};

/** TODO 樊永晖
 * 获取当前账户
 */
export const getAccount = async function () {
    try {
        return await ethereum.request({
            method: 'eth_requestAccounts',
        });
    } catch (error) {
        console.error(error);
    }
};
/** TODO 樊永晖
 * 查询账户eth余额
 * @param address 钱包地址
 * @returns {Promise<BigNumber>}
 */
export const getBalance = async function (address) {

    try {
        let res = await Web3.eth.getBalance(address, "latest");
        // loading.close()
        return res
    } catch (error) {

        onConnect()
    }
};

/** TODO 马文翼
 * 发送ETH
 * @param receiveAddress 接收人地址
 * @param value 转账数量
 * @param gasLimit gas费限制
 * @param gasPrice gas费价格
 */
export const sendEth = async function (receiveAddress, value, gasLimit = 21000, gasPrice = 20000000000) {
    if (isMetaMaskInstalled) {
        return await ethersProvider.getSigner().sendTransaction({
            to: receiveAddress,
            value: value,
            gasLimit: gasLimit,
            gasPrice: gasPrice,
        });
    } else {
        return false
    }
};
/** TODO 赵鑫哲
 * 发送合约
 * @param receiveAddress
 * @param data
 * @param value
 * @param gasLimit
 * @param gasPrice
 * @returns {Promise<TransactionResponse>}
 */
export const sendContact = async function (receiveAddress, data, value = 0, gasPrice = 20000000000 , gasLimit = 210000) {
    if (isMetaMaskInstalled) {
        return await ethersProvider.getSigner().sendTransaction({
            to: receiveAddress,
            value: "0x"+value.toString(16),
            data: data,
            gasLimit: gasLimit,
            gasPrice: gasPrice,
        });
    } else {
        return false
    }
};
/** TODO 陈鹏
 * 添加代币
 * @param type
 * @param contractAddress
 * @param tokenSymbol
 * @param decimalUnits
 * @param image
 * @returns {Promise<*>}
 */
export const addTokenToWallet = async (type = 'ERC20', contractAddress, tokenSymbol, decimalUnits = 0, image = 'https://metamask.github.io/test-dapp/metamask-fox.svg') => {
    return await ethereum.request({
        method: 'wallet_watchAsset',
        params: {
            type: type,
            options: {
                address: contractAddress,
                symbol: tokenSymbol,
                decimals: decimalUnits,
                image: image,
            },
        },
    });
};
/** TODO 陈鹏
 * 创建合约方法对象
 * @param contractAbi
 * @param contractAddress
 * @param currentAccount
 * @returns {Promise<*>}
 */
export const contactMethod = async function (contractAbi, contractAddress, currentAccount) {
    let myContract = new Web3.eth.Contract(contractAbi, contractAddress, { from: currentAccount });
    return myContract.methods;
}
/** TODO 关大伟
 * 添加网络
 * @returns {Promise<void>}
 */
export const addChain = async (chainId = '0x64', rpcUrls = ['https://dai.poa.network'], chainName = 'xDAI Chain', nativeCurrency = {
    name: 'xDAI',
    decimals: 18,
    symbol: 'xDAI'
}, blockExplorerUrls = ['https://blockscout.com/poa/xdai']) => {
    try {
        await ethereum.request({
            method: 'wallet_switchEthereumChain',
            params: [{ chainId: chainId }],
        });
    } catch (switchError) {
        // This error code indicates that the chain has not been added to MetaMask.
        if (switchError.code === 4902) {
            try {
                await ethereum.request({
                    method: 'wallet_addEthereumChain',
                    params: [{
                        chainId: chainId,
                        rpcUrls: rpcUrls,
                        chainName: chainName,
                        nativeCurrency: nativeCurrency,
                        blockExplorerUrls: blockExplorerUrls,
                    }],
                });
            } catch (addError) {
                // handle "add" error
                console.log("addErroraaa", addError);
                return addError
            }
        } else{
            console.log("switchError", switchError);
            return switchError
        }
    }
};
/** TODO 关大伟
 * 切换网络
 * @returns {Promise<void>}
 */
export const switchChain = async (chainId = '0x04') => {
    await ethereum.request({
        method: 'wallet_switchEthereumChain',
        params: [
            {
                chainId: chainId,
            },
        ],
    });
};
