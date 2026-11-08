#include<bits/stdc++.h>
using namespace std;

#define int         long long
#define ull         unsigned long long
#define ll             long long
#define pb             push_back
#define all(a)      (a).begin(),(a).end()
#define sz(x)       (ll)x.size()
#define endl        '\n'
#define gcd(a,b)    __gcd((a),(b))
#define lcm(a,b)    ((a)*(b)) / gcd((a),(b))
#define ios            ios_base::sync_with_stdio(false);cin.tie(0);cout.tie(0);
#define mp             make_pair
#define lb             lower_bound
#define ub            upper_bound
#define F           first
#define S           second
#define rep(i, begin, end) for (__typeof(end) i = (begin) - ((begin) > (end)); i != (end) - ((begin) > (end)); i += 1 - 2 * ((begin) > (end)))
#define hell         (ull)1e9

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
    int N;
    cin >> N;
    vector<int> arr(N,0);
    rep(i,0,N) cin >> arr[i];

    int me = 0;
    int mo = INT_MAX;

    rep(i,0,N)
    {
        if(arr[i]%2)
        {
            mo = min(mo, arr[i]);
        }
        else
        {
            me = max(me, arr[i]);
        }
    }

    int de=0,dod=0;

    rep(i,0,N)
    {
        if(arr[i]%2)
        {
            dod += arr[i]-mo;
        }
        else
        {
            de += me-arr[i];
        }
    }

    if(abs(dod-de) % N == 0) cout << "YES\n";
    else cout << "NO\n";
}

