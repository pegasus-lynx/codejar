#include<bits/stdc++.h> 
using namespace std;

#define int         long long
#define ull         unsigned long long
#define ll          long long
#define pb          push_back
#define all(a)      (a).begin(),(a).end()
#define sz(x)       (ll)x.size()
#define endl        '\n'
#define gcd(a,b)    __gcd((a),(b))
#define lcm(a,b)    ((a)*(b)) / gcd((a),(b))
#define ios         ios_base::sync_with_stdio(false);cin.tie(0);cout.tie(0);
#define mp          make_pair
#define lb          lower_bound
#define ub          upper_bound
#define F           first
#define S           second
#define rep(i, begin, end) for (__typeof(end) i = (begin) - ((begin) > (end)); i != (end) - ((begin) > (end)); i += 1 - 2 * ((begin) > (end)))
#define hell        (ull)1e9

void solve();

signed main(void)
{
    ios
    freopen("input.txt", "r", stdin);freopen("output.txt", "w", stdout);
    int TESTS=1;
    cin>>TESTS;

    while(TESTS--){
        solve();
    }
    return 0;
}

void solve()
{
    int n,x,y, blimps=0;
    cin >> n >> x >> y;
    vector<int> arr(n,0);
    rep(i,0,n) cin >> arr[i];
    if(x <= y)
    {
        int ms = *max_element(all(arr));
        // cout << "max : " << ms;
        blimps = ms % y != 0 ? (ms/y)+1 : ms / y;
    }
    else
    {
        int ydec = 0, p = n-1;
        while(p>=0)
        {
            if(arr[p] <= ydec)
            {
                p--;
                continue;
            }

            arr[p] -= ydec;
            int cb = arr[p] % x != 0 ? (arr[p]/x)+1 : arr[p]/x;
            ydec += y*cb;
            blimps += cb;
            p--;
        }
    }

    cout << blimps << "\n";
}


