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
    int n,q;
    cin >> n >> q;
    vector<int> arr(n,0);
    vector<int> bits(33,0);

    rep(i,0,n)
    {
        cin >> arr[i];
        int x = arr[i];
        int j = 0;
        while(x>0)
        {
            bits[j++] += x & 1;
            x = x >> 1;
        }
    }

    // rep(i,0,33) cout << bits[i] << " ";
    // cout << "\n";

    int ans = 0;
    rep(i,0,33)
    {
        if(bits[i]>0)
        {
            ans |= (1<<i);
        }
    }

    cout << ans << "\n";

    int x,v;
    rep(i,0,q)
    {
        cin >> x >> v;
        x--;
        int t = arr[x];
        int j = 0;
        while(t>0)
        {
            bits[j++] -= t & 1;
            t = t >> 1;
        }

        arr[x] = v;
        j=0;
        while(v>0)
        {
            bits[j++] += v & 1;
            v = v >> 1;
        }

        int ans = 0;
        rep(j,0,33)
        {
            if(bits[j]>0)
            {
                ans |= (1<<j);
            }
        }

        cout << ans << "\n";
    }
}

